using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private int minZ = 100;
    private int maxZ = 200;
    private int minX = -3;
    private int maxX = 3;
    public new GameObject gameObject;
    private Transform playerTransform;
    private int LoadingDistance = 210;

    private float safeZone = 75.0f;
    private List<GameObject> obstacleList;


    // Start is called before the first frame update
    void Start()
    {
        obstacleList = new List<GameObject>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        
    }

    private void LoadObstacles()
    {
        
        for (int i = minZ; i < maxZ; i += 25)
        {
            Vector3 randomPos = new Vector3(Random.Range(minX, maxX), 0, i);
            Instantiate(gameObject, randomPos, Quaternion.identity);
        }

        obstacleList.Add(gameObject);
        
        minZ += LoadingDistance;
        maxZ += LoadingDistance;
    
    }

    private void Update()
    {        
        if (minZ -  playerTransform.position.z <  safeZone )
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
