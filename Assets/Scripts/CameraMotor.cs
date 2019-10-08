using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraMotor : MonoBehaviour
{
    public Transform lookAt; // our player
    public Vector3 offset;
    private Vector3 initialOffset = new Vector3(0, 5.0f, -4.0f);
    public Vector3 rotation = new Vector3(35, 0, 0);

    // Shakes the camera
    public float shakeDuration = 0f;
    public float shakeAmount = 0.2f;

    // Wait for player
    public bool isFollowing = false;

    private void Start()
    {
        // Remove the if statemtnt if you want your scenes to have the start menu Forest level has
        if (!SceneManager.GetActiveScene().name.Equals("Forest"))
            transform.position = lookAt.position + initialOffset;
    } 

    private void LateUpdate()
    {
        if (!isFollowing)
        {
            return;
        }

        Vector3 desiredPos = lookAt.position + offset;
        if (shakeDuration > 0)
        {
            Vector3 shakePos = Random.insideUnitSphere;
            Vector3 targetPos = Vector3.zero;
            targetPos.z = Mathf.Lerp(transform.position.z, desiredPos.z, Time.deltaTime) + shakePos.z * shakeAmount;
            targetPos.y = Mathf.Lerp(transform.position.y, desiredPos.y, Time.deltaTime) + shakePos.y * shakeAmount;
            targetPos.x = Mathf.Lerp(transform.position.x, desiredPos.x, Mathf.SmoothStep(0.0f, 1.0f, Time.deltaTime)) + shakePos.x * shakeAmount;

            transform.position = targetPos;

            shakeDuration -= Time.deltaTime;
        } else
        {
            Vector3 targetPos = Vector3.zero;
            targetPos.z = Mathf.Lerp(transform.position.z, desiredPos.z, Time.deltaTime);
            targetPos.y = Mathf.Lerp(transform.position.y, desiredPos.y, Time.deltaTime);

            targetPos.x = Mathf.SmoothStep(transform.position.x, desiredPos.x, Time.deltaTime * 7);


            transform.position = targetPos;
        }
        if (SceneManager.GetActiveScene().name.Equals("Forest"))
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rotation), 0.05f);

    }

    public void StartFollowing()
    {
        isFollowing = true;
    }

}
