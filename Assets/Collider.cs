using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collider : MonoBehaviour
{
    public GameObject[] gameObjects;
    private Transform playerTransform;
    private int nextLoadingDistanceIncrement = 20;
    private static List<GameObject> obstacleList;
    private int triggerDistance = 10;
    private float nextObstacleZLocation;
    private CameraMotor cameraMotor;


    private float lengthOfLevelTile = 100f;
    // Start is called before the first frame update
    void Start()
    {
        cameraMotor = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMotor>();

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        
    }

    private void Awake()
    {
        obstacleList = new List<GameObject>();
    }

    private void LoadNextObstacle(float colliderPosition)
    {
        cameraMotor.shakeDuration = 1f;
        int objectIndex = Random.Range(0, gameObjects.Length);
        nextObstacleZLocation = colliderPosition + 38;
        float xcord = 21;

        if (objectIndex == 0)
        {
            xcord = 45;
        }
        
        Vector3 pos = new Vector3(xcord, 0, nextObstacleZLocation);
        GameObject go = Instantiate(gameObjects[objectIndex], pos, gameObjects[objectIndex].transform.rotation);
        obstacleList.Add(go);
    }

    private void Update()
    {        
   
    }

    private void DeleteObstacles()
    {
        if (obstacleList.Count > 1)
        {
            Destroy(obstacleList[0]);
            obstacleList.RemoveAt(0);
        }
    }

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        LoadNextObstacle(other.transform.position.z);
        DeleteObstacles();
    }

}
