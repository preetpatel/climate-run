using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icebreaker : MonoBehaviour
{
    private Transform lookAt; // our player
    private Vector3 offset;

    private void Start()
    {
        lookAt = GameObject.FindGameObjectWithTag("Player").transform;
        offset = new Vector3(0, 0, 5.0f);
    }

    // Ice breaks as player moves forward
    private void LateUpdate()
    {
        if (lookAt.position.z > gameObject.transform.position.z) // wait for player to move
        {
            Vector3 desiredPos = lookAt.position + offset;
            Vector3 targetPos = Vector3.zero;
            targetPos.z = Mathf.Lerp(transform.position.z, desiredPos.z, Time.deltaTime);
            targetPos.y = transform.position.y;
            targetPos.x = transform.position.x;
            transform.position = targetPos;
        }
    }
}