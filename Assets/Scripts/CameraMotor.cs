using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    public Transform lookAt; // our player
    public Vector3 offset;
    private Vector3 initialOffset = new Vector3(0, 5.0f, -4.0f);

    // Shakes the camera
    public float shakeDuration = 0f;
    public float shakeAmount = 0.2f;

    // Wait for player
    private bool isFollowing = false;

    private void Start()
    {
        transform.position = lookAt.position + initialOffset;
    } 

    private void LateUpdate()
    {
        if (!isFollowing)
        {
            return;
        }

        Vector3 desiredPos = lookAt.position + offset;
        desiredPos.x = 0;
        if (shakeDuration > 0)
        {
            transform.position = Vector3.Lerp(transform.position, desiredPos, Time.deltaTime) + Random.insideUnitSphere * shakeAmount;
            shakeDuration -= Time.deltaTime;
        } else
        {
            transform.position = Vector3.Lerp(transform.position, desiredPos, Time.deltaTime);
        }

    }

    public void StartFollowing()
    {
        isFollowing = true;
    }

}
