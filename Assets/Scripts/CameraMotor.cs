using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    public Transform lookAt; // our player
    public Vector3 offset = new Vector3(0, 2.5f, -1.0f);

    private void Start()
    {
        transform.position = lookAt.position + offset;
    } 
    private void LateUpdate()
    {
        Vector3 desiredPos = lookAt.position + offset;
        desiredPos.x = 0;
        transform.position = Vector3.Lerp(transform.position, desiredPos, Time.deltaTime);
    }

}
