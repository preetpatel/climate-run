using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GorillaMotor : MonoBehaviour
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
    private bool finishing = false;
    private bool done = false;
    private bool haveSetSlow = false;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        if (!isRunning)
        {
            return;
        }

        // Calculate where we will be in the future
        Vector3 desiredPos = playerTransform.position;
        Vector3 targetPosition = Vector3.zero;

        if (!finishing)
        {
            targetPosition.x = Mathf.SmoothStep(transform.position.x, desiredPos.x, Time.deltaTime * 6);
            targetPosition.y = Mathf.Lerp(transform.position.y, desiredPos.y, Time.deltaTime * 2);
            targetPosition.z = Mathf.Lerp(transform.position.z, desiredPos.z, Time.deltaTime * 2);
        }
        else if (!done)
        {
            // walking towards target
            
            targetPosition.x = Mathf.Lerp(transform.position.x, desiredPos.x, Time.deltaTime * 0.5f);
            targetPosition.y = Mathf.Lerp(transform.position.y, desiredPos.y, Time.deltaTime * 0.5f);
            targetPosition.z = Mathf.Lerp(transform.position.z, desiredPos.z, Time.deltaTime * 0.5f);

            if ((transform.position - desiredPos).magnitude < 4f && !haveSetSlow)
            {
                haveSetSlow = true;
                anim.SetTrigger("WalkSlowly");
            }

            if ((transform.position - desiredPos).magnitude < 2f)
            {
                done = true;
                anim.SetTrigger("Stop");
                transform.SetPositionAndRotation(targetPosition, 
                    Quaternion.LookRotation(new Vector3(0.5f,0,-0.5f), Vector3.up));
                return;
            }
        }

        if (!done)
        {
            Vector3 moveVector = targetPosition - transform.position;
            moveVector.Normalize();

            // Move the actual character
            transform.SetPositionAndRotation(targetPosition, Quaternion.LookRotation(moveVector, Vector3.up));
        }

    }

    public void StartRunning()
    {
        isRunning = true;
        anim.SetTrigger("StartFollow");
    }

    public void DoEndSequence(Transform gorillaPosition)
    {
        Debug.Log("doing end sequence");
        anim.SetTrigger("Walk");
        playerTransform = gorillaPosition;
        finishing = true;
    }

}
