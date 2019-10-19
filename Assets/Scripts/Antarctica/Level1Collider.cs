using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Collider : MonoBehaviour
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

        float loadingPosition = colliderPosition + 50;

        int[] xcords = new int[] {-3, 0, 3};
        
        for (int i = (int) loadingPosition; i < loadingPosition + 80; i += 20)
        {
            int xcord = xcords[Random.Range(0, 3)];
            Vector3 randomPos = new Vector3(xcord, 0.5f, i);
            GameObject go = Instantiate(gameObjects[0], randomPos, gameObjects[0].transform.rotation);
            obstacleList.Add(go);        
        }
        
    
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
