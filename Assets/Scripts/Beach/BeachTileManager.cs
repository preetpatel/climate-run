using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeachTileManager : MonoBehaviour
{
    public List<GameObject> tilePrefabs;

    private Transform playerTransform;

    private float spawnZ = 0.0f;

    private float levelLength = 100.0f;

    private int amnLevelsOnScreen = 3;
    private float safeZone = 110.0f;
    private int lastPrefabIndex = 0;
    private int nextLevelSpawnLocation = 60;
    private float nextLevelZLocation = 0.0f;
    
    
    private List<GameObject> activeLevels;
    private List<GameObject> instantiatedLevels;

    // Start is called before the first frame update
    private void Start()
    {
        activeLevels = new List<GameObject>();
        instantiatedLevels = new List<GameObject>();
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
        //spawns the background tile when the penguin has reached a certain position
        List<GameObject> possibleTiles = null;
        possibleTiles = instantiatedLevels.FindAll(
            x => !x.gameObject.activeSelf);
        Vector3 pos = new Vector3(0, 0, nextLevelZLocation);

        if (possibleTiles == null || possibleTiles.Count == 0)
        {
            int tileToSpawn = Random.Range(0, tilePrefabs.Count);
            GameObject go = Instantiate(tilePrefabs[tileToSpawn], pos, Quaternion.identity);
            activeLevels.Add(go);
            instantiatedLevels.Add(go);
        } else 
        {
            int tileToSpawn = Random.Range(0, possibleTiles.Count);
            GameObject go = possibleTiles[tileToSpawn];
            go.transform.position = pos;
            go.SetActive(true);
            activeLevels.Add(go);
        }



        nextLevelZLocation += levelLength;
    }

    public void DeleteLevel()
    {
        activeLevels[0].SetActive(false);
        activeLevels.RemoveAt(0);
    }
}
