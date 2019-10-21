using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMotor : MonoBehaviour
{
    private const float LANE_DISTANCE = 2.5f;
    private const float TURN_SPEED = 0.05F;

    // Animation
    private Animator anim;

    // Wait for player
    private bool isRunning = false;
    private bool walkingTowardsTarget = false;
    private Transform walkTarget;

    // Movements
    private CharacterController controller;
    private float jumpForce = 6.0f;
    private float gravity = 12.0f;
    private float verticalVelocity = 0;
    private bool DID_SWIPE_UP;

    // speed modifier
    public float originalSpeed = 7.0f;
    public float speed;
    private float speedIncreaseLastTick;
    private float speedIncreaseTime = 2.5f;
    private float speedIncrement = 0.1f;
    private int livesCounter = 3;

    // 0 is left, 1 is middle, 2 is right
    private int lane = 1;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        speed = originalSpeed;
    }

    private void Update()
    {
        if (walkingTowardsTarget)
        {
            MoveToCutscenePos();
            return;
        }

        if (!isRunning)
        {
            return;
        }

        if (Time.time - speedIncreaseLastTick > speedIncreaseTime)
        {
            speedIncreaseLastTick = Time.time;
            speed += speedIncrement;

            Scene gameScene = SceneManager.GetActiveScene();
            if (gameScene.name.Equals("Forest"))
            {
                // ForestLevelManager.Instance.updateLives(speed - originalSpeed);
            } else if (gameScene.name.Equals("Beach"))
            {
                // Add speed increments for Beach
            } else if (gameScene.name.Equals("Antarctica"))
            {
                // Add speed increments for Antarctica
            }

        }
        // Check which lane we should be
        if(MobileInput.Instance.SwipeLeft)
        {
            MoveLane(false);
        }
        if(MobileInput.Instance.SwipeRight)
        {
            MoveLane(true);
        }

        // Calculate where we will be in the future
        Vector3 targetPosition = transform.position.z * Vector3.forward;
        if(lane == 0)
        {
            targetPosition += Vector3.left * LANE_DISTANCE;
        } else if (lane == 2)
        {
            targetPosition += Vector3.right * LANE_DISTANCE;
        }

        // Calculating our move vector
        Vector3 moveVector = Vector3.zero;

        // where we should be - where we are to get a normalised vector.
        if(Mathf.Abs((targetPosition - transform.position).x) > 0.1)
            moveVector.x = (targetPosition - transform.position).normalized.x * speed;

        bool isGrounded = IsGrounded();
        anim.SetBool("Grounded", isGrounded);

        // Calculate Y
        if (isGrounded) // if grounded
        {
            DID_SWIPE_UP = false;
            verticalVelocity = 0.0f;

            if (MobileInput.Instance.SwipeUp && !DID_SWIPE_UP)
            {
                DID_SWIPE_UP = true;
                // Jump
                anim.SetTrigger("Jump");
                verticalVelocity = jumpForce;
            }
            else if (MobileInput.Instance.SwipeDown)
            {
                // Slide
                StartSliding();
                Invoke("StopSliding", 1.0f);
            }
        }
        else // fast fall
        {
            if (MobileInput.Instance.SwipeUp && !DID_SWIPE_UP)
            {
                DID_SWIPE_UP = true;
                // Jump
                anim.SetTrigger("Jump");
                verticalVelocity = jumpForce;
            }
            else if (MobileInput.Instance.SwipeDown)
            {
                // Slide
                StartSliding();
                Invoke("StopSliding", 1.0f);
            }
            else
            {
                verticalVelocity -= (gravity * Time.deltaTime);

                if (MobileInput.Instance.SwipeDown)
                {
                    verticalVelocity = -jumpForce;
                }
            }
        }

        moveVector.y = verticalVelocity;
        moveVector.z = speed;

        // Move the actual character
        controller.Move(moveVector * Time.deltaTime);

        // Rotate the character while moving
        Vector3 dir = controller.velocity;
        if (dir != Vector3.zero)
        {
            dir.y = 0;
            transform.forward = Vector3.Lerp(transform.forward, dir, TURN_SPEED);
        }

        // Player falls through level
        if (transform.position.y < 0)
        {
            Crash();
            //transform.position = new Vector3(0, 0, 0);
        }
    }

    private void MoveToCutscenePos()
    {
        Vector3 targetPos = Vector3.zero;
        Vector3 desiredPos = walkTarget.position;

        targetPos.x = Mathf.Lerp(transform.position.x, desiredPos.x, Time.deltaTime * 0.5f);
        targetPos.y = Mathf.Lerp(transform.position.y, desiredPos.y, Time.deltaTime * 0.5f);
        targetPos.z = Mathf.Lerp(transform.position.z, desiredPos.z, Time.deltaTime * 0.5f);

        Quaternion targetDir = Quaternion.Slerp(transform.rotation, walkTarget.rotation, Time.deltaTime * 2);

        transform.SetPositionAndRotation(targetPos, targetDir);

        if (Mathf.Abs((transform.position - desiredPos).magnitude) < 2f)
        {
            walkingTowardsTarget = false;
            anim.SetTrigger("StopRunning");
            return;
        }
    }

    private void MoveLane(bool goRight)
    {

        // Switches lanes, and is clamped between 0 and 2
        lane += (goRight) ? 1 : -1;
        lane = Mathf.Clamp(lane, 0, 2);
    }

    private bool IsGrounded()
    {
        Ray groundRay = new Ray(
            new Vector3(
                controller.bounds.center.x,
                (controller.bounds.center.y - controller.bounds.extents.y) + 0.2f,
                controller.bounds.center.z),
                Vector3.down
        );

        return Physics.Raycast(groundRay, 0.2f + 0.1f);
    }

    public void ResetMotorBeforeSceneLoad()
    {
        isRunning = false;
    }

    public void StartRunning()
    {
        isRunning = true;
        anim.SetTrigger("StartRunning");
    }

    public void StopRunning()
    {
        isRunning = false;
        anim.SetTrigger("StopRunning");
    }

    public void StartSliding()
    {
        anim.SetBool("Sliding", true);
        controller.height /= 2;
        controller.center = new Vector3(controller.center.x, controller.center.y / 2, controller.center.z);
    }

    public void StopSliding()
    {
        anim.SetBool("Sliding", false);
        controller.height *= 2;
        controller.center = new Vector3(controller.center.x, controller.center.y * 2, controller.center.z);
    }

    private void Crash()
    {

        livesCounter -= 1;
        GameObject audioPlayer = GameObject.FindGameObjectWithTag("SoundController");
        Music sfx = audioPlayer.GetComponent<Music>();
        // If no more lives are left, do a crash
        if (livesCounter < 1)
        {
            anim.SetTrigger("Death");
            isRunning = false;

            if (Settings.isSfxOn.Value)
            {
                sfx.playGameOver();
            }
            if (SceneManager.GetActiveScene().name.Equals("Forest"))
            {
                ForestLevelManager.Instance.OnDeath();
            } else if (SceneManager.GetActiveScene().name.Equals("Beach"))
            {
                BeachLevelManager.Instance.OnDeath();
            } else if (SceneManager.GetActiveScene().name.Equals("Antarctica"))
            {
                AntarcticaLevelManager.Instance.OnDeath();
            }
        }
        else // Otherwise if we still have lives remaining, move the character up and give another chance
        {
            anim.SetTrigger("Jump");
            verticalVelocity = jumpForce;
            Vector3 hitButRevert = new Vector3(0, 2, 3);
            CameraMotor cameraMotor = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMotor>();
            controller.Move(hitButRevert);
            cameraMotor.shakeDuration = 0.5f;

            if(Settings.isSfxOn.Value)
            {
                sfx.playDamage();
            }
            // Update lives for forest
            Scene gameScene = SceneManager.GetActiveScene();
            if (gameScene.name.Equals("Forest"))
            {
                StartCoroutine(ForestLevelManager.Instance.updateLives(livesCounter));
            } else if (gameScene.name.Equals("Beach"))
            {
                StartCoroutine(BeachLevelManager.Instance.updateLives(livesCounter));
            } else
            {
                StartCoroutine(AntarcticaLevelManager.Instance.updateLives(livesCounter));
            }
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        switch (hit.gameObject.tag)
        {
            case "Obstacle":
				hit.collider.enabled = false;
                Crash();
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "FireTruck")
        {
            GameObject segment = other.gameObject.transform.parent.gameObject;
            FireTruckAction sprayScript = segment.GetComponent<FireTruckAction>();
            sprayScript.doWaterSpray();
        }
        else if (other.gameObject.tag == "ForestFinish")
        {
            GameObject triggerParent = other.gameObject.transform.parent.gameObject;
            ForestLevelManager.Instance.StopGameIfNotEndless(triggerParent);
            walkTarget = triggerParent.transform.Find("FlippyPosition");
            walkingTowardsTarget = true;
            isRunning = false;
        }
    }
}
