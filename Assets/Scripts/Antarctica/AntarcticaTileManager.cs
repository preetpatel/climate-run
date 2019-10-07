using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AntarcticaTileManager : MonoBehaviour
{
    public GameObject[] levelTilePrefabs;

    private Transform playerTransform;

    private float distanceLevelHasReached = 0.0f;

    private float levelTileLength = 100.0f;

    private int levelTilesSpawnedOnScreen = 2;

    private float levelTileSpawnZone = 110.0f;

    private int lastLevelTileIndex = 0;
    
    
    private List<GameObject> activeLevels;
    // Start is called before the first frame update
    private void Start()
    {
        activeLevels = new List<GameObject>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        for (int i = 0; i < levelTilesSpawnedOnScreen; i++)
        {
            SpawnLevelTile();
        }
       
    }

    //Once per frame this method is called
    private void Update()
    {
        // Check if it is suitable to load the next level tile
        if (playerTransform.position.z - levelTileSpawnZone > (distanceLevelHasReached - levelTilesSpawnedOnScreen * levelTileLength))
        {
            SpawnLevelTile();
            DeletePreviousLevelTile();
        }
    }
    
    // Generate new level tile
    private void SpawnLevelTile()
    {
        // Get the next level tile
        GameObject gameObject;
        gameObject = Instantiate(levelTilePrefabs[lastLevelTileIndex]) as GameObject;
        gameObject.transform.SetParent(transform);

        // Setup the next one 
        if (lastLevelTileIndex == 0)
        {
            lastLevelTileIndex = 1;
        }
        else
        {
            lastLevelTileIndex = 0;
        }
        
        // Position the tile level
        gameObject.transform.position = Vector3.forward * distanceLevelHasReached;
        distanceLevelHasReached += levelTileLength;
        activeLevels.Add(gameObject);

    }

    // Destroy and delete the previous tile level
    public void DeletePreviousLevelTile()
    {
        Destroy(activeLevels[0]);
        activeLevels.RemoveAt(0);
    }
}
