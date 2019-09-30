using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private const float LANE_DISTANCE = 3.0f;

    // Movements
    private CharacterController controller;
    private float jumpForce = 6.0f;
    private float gravity = 12.0f;
    private float verticalVelocity;
    private float speed = 7.0f;

    // 0 is left, 1 is middle, 2 is right
    private int lane = 1;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // Check which lane we should be
        if(Input.GetKeyDown(KeyCode.A))
        {
            MoveLane(false);
        }
        if(Input.GetKeyDown(KeyCode.D))
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
        moveVector.x = (targetPosition - transform.position).normalized.x * speed;
        moveVector.y = -0.1f;
        moveVector.z = speed;

        // Move the actual character
        controller.Move(moveVector * Time.deltaTime);
    }

    private void MoveLane(bool goRight)
    {

        // Switches lanes, and is clamped between 0 and 2
        lane += (goRight) ? 1 : -1;
        lane = Mathf.Clamp(lane, 0, 2);
    }   
}
