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

    private bool transitionToCutscenePos = false;
    private Transform cutscenePos;

    private void Start()
    {
    } 

    private void LateUpdate()
    {
        if (transitionToCutscenePos)
        {
            Vector3 targetPos = Vector3.zero;

            targetPos.x = Mathf.SmoothStep(transform.position.x, cutscenePos.position.x, Time.deltaTime * 4);
            targetPos.y = Mathf.SmoothStep(transform.position.y, cutscenePos.position.y, Time.deltaTime * 4);
            targetPos.z = Mathf.SmoothStep(transform.position.z, cutscenePos.position.z, Time.deltaTime * 4);

            Quaternion targetDir = Quaternion.Slerp(transform.rotation, cutscenePos.rotation, Time.deltaTime * 4);

            transform.SetPositionAndRotation(targetPos, targetDir);

            if ((targetPos - cutscenePos.position).magnitude < 1.0f)
            {
                transitionToCutscenePos = false;
            }

            return;
        }

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
        if (SceneManager.GetActiveScene().name.Equals("Forest") || SceneManager.GetActiveScene().name.Equals("Beach"))
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rotation), 0.05f);

    }

    public void StartFollowing()
    {
        isFollowing = true;
    }

    public void StopFollowing()
    {
        isFollowing = false;
    }

    public void MoveToCutscenePos(Transform position)
    {
        isFollowing = false;
        transitionToCutscenePos = true;
        cutscenePos = position;
    }
}
