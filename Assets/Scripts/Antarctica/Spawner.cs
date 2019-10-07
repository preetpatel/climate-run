using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Spawns the obstacles for user to avoid
public class Spawner : MonoBehaviour
{
    private int minZ = 100;
    private int maxZ = 200;
    private int minX = -3;
    private int maxX = 3;
    public new GameObject gameObject;
    private Transform playerTransform;
    private int LoadingDistance = 210;

    private float triggerObstaclesZone = 75.0f;
    private List<GameObject> obstacleList;


    // Method called before first frame
    void Start()
    {
        obstacleList = new List<GameObject>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        
    }

    // Loads the obstacles when required
    private void LoadObstacles()
    {
        // Loads the obstacles in random lanes but at a constant distance
        for (int i = minZ; i < maxZ; i += 25)
        {
            Vector3 randomPos = new Vector3(Random.Range(minX, maxX), 0, i);
            Instantiate(gameObject, randomPos, Quaternion.identity);
        }
           
        obstacleList.Add(gameObject);
        
        // Set up the next obstacles to be loaded
        minZ += LoadingDistance;
        maxZ += LoadingDistance;
    
    }

    private void Update()
    {        
        if (minZ -  playerTransform.position.z <  triggerObstaclesZone )
        {
            LoadObstacles();
            DeleteObstacles();
    
        }
    }

    private void DeleteObstacles()
    {
        Destroy(obstacleList[0]);
        obstacleList.RemoveAt(0);
    }
}
