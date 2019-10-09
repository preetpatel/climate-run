using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeachTileManager : MonoBehaviour
{
    public GameObject[] tilePrefabs;

    private Transform playerTransform;

    private float spawnZ = 0.0f;

    private float levelLength = 100.0f;

    private int amnLevelsOnScreen = 3;
    private float safeZone = 110.0f;
    private int lastPrefabIndex = 0;
    private int nextLevelSpawnLocation = 60;
    private float nextLevelZLocation = 0.0f;
    
    
    private List<GameObject> activeLevels;
    // Start is called before the first frame update
    private void Start()
    {
        activeLevels = new List<GameObject>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        for (int i = 0; i < amnLevelsOnScreen; i++)
        {
            SpawnLevel();
        }
       
    }

   

    // Update is called once per frame
    private void Update()
    {
        if (playerTransform.position.z >= nextLevelSpawnLocation)
        {
            nextLevelSpawnLocation += 100;
            SpawnLevel();
            DeleteLevel();
        }
    }

    private void SpawnLevel()
    {
        GameObject go;
        Vector3 pos = new Vector3(0, 0, nextLevelZLocation);
        go = Instantiate(tilePrefabs[0], pos, Quaternion.identity);
        nextLevelZLocation += levelLength;
        activeLevels.Add(go);
    }

    public void DeleteLevel()
    {
        Destroy(activeLevels[0]);
        activeLevels.RemoveAt(0);
    }
}
