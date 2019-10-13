using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Spawns the obstacles for user to avoid
public class Spawner : MonoBehaviour
{
    public GameObject[] gameObjects;
    private Transform playerTransform;
    private int nextLoadingDistanceIncrement = 40;
    private List<GameObject> obstacleList;
    private int triggerDistance = 20;
    private int nextObstacleZLocation = 40;

    private float lengthOfLevelTile = 100f;
    // Start is called before the first frame update
    void Start()
    {
        obstacleList = new List<GameObject>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void LoadNextObstacle()
    {
        int objectIndex = Random.Range(0, gameObjects.Length);

        Vector3 pos = new Vector3(10, 0, nextObstacleZLocation);
        GameObject go = Instantiate(gameObjects[objectIndex], pos, gameObjects[objectIndex].transform.rotation);
        obstacleList.Add(go);
        nextObstacleZLocation += 40;
    }


    private void Update()
    {        
        if (playerTransform.position.z >= triggerDistance && playerTransform.position.z <     lengthOfLevelTile )
        {
            LoadNextObstacle();
            triggerDistance += nextLoadingDistanceIncrement;
        }
    }

    private void DeleteObstacles()
    {
        Destroy(obstacleList[0]);
        obstacleList.RemoveAt(0);
    }
}
