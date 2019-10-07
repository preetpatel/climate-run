using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Spawns the side display
public class SideObjectSpawner : MonoBehaviour
{
    public static SideObjectSpawner Instance { set; get; }
    private const float DISTANCE_TO_DESPAWN_OBJECT = 10f;
    public float scrollSpeed = -2;
    public float totalLength;
    public bool IsScrolling { get; set; }

    private float scrollLocation;
    private Transform playerTransform;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Updates the side objects
    private void Update()
    {
        // Only updates when still playing
        if (!IsScrolling)
        {
            return;
        }
        scrollLocation += scrollSpeed * Time.deltaTime;
        Vector3 newLocation = (playerTransform.position.z + scrollLocation) * Vector3.forward;
        transform.position = newLocation;

        // If the user has travelled far enough, spawn new side objects
        if (transform.GetChild(0).transform.position.z < playerTransform.position.z - DISTANCE_TO_DESPAWN_OBJECT)
        {
            transform.GetChild(0).localPosition += Vector3.forward * totalLength;
            transform.GetChild(0).SetSiblingIndex(transform.childCount);

            transform.GetChild(0).localPosition += Vector3.forward * totalLength;
            transform.GetChild(0).SetSiblingIndex(transform.childCount);
        }
    }
}
