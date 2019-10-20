using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CompanionMotor : MonoBehaviour
{
    private const float LANE_DISTANCE = 2.5f;
    private const float TURN_SPEED = 0.05F;

    // Animation
    private Animator anim;

    // Wait for player
    private bool isRunning = false;

    // Movements
    private CharacterController controller;
    private Transform playerTransform;
    private float jumpForce = 6.0f;
    private float gravity = 12.0f;
    private float verticalVelocity = 0;


    // speed modifier
    private float originalSpeed = 7.0f;
    private float speed;
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
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        speed = originalSpeed;
    }

    private void Update()
    {
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
            }
            else if (gameScene.name.Equals("Beach"))
            {
                // Add speed increments for Beach
            }
            else if (gameScene.name.Equals("Antarctica"))
            {
                // Add speed increments for Antarctica
            }

        }
        // Check which lane we should be
        if (MobileInput.Instance.SwipeLeft)
        {
            MoveLane(false);
        }
        if (MobileInput.Instance.SwipeRight)
        {
            MoveLane(true);
        }

        // Calculate where we will be in the future
        Vector3 targetPosition = playerTransform.position;
        targetPosition.z -= 2;


        // Calculating our move vector
        Vector3 moveVector = Vector3.zero;

        // where we should be - where we are to get a normalised vector.
        if (Mathf.Abs((targetPosition - transform.position).x) > 0.08)
            moveVector.x = (targetPosition - transform.position).normalized.x * speed;

        moveVector.y = targetPosition.y-transform.position.y;
        moveVector.z = speed;

        // Move the actual character
        /*controller.Move(moveVector * Time.deltaTime);*/
        transform.SetPositionAndRotation(targetPosition, Quaternion.Euler(new Vector3(0, 0, 0)));

    }

    private void MoveLane(bool goRight)
    {

        // Switches lanes, and is clamped between 0 and 2
        lane += (goRight) ? 1 : -1;
        lane = Mathf.Clamp(lane, 0, 2);
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

}
