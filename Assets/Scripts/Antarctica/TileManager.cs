using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * In the level, loads the level tiles (different parts of the levels) one after another to make the level endless 
 */
public class TileManager : MonoBehaviour
{
    public GameObject[] tilePrefabs;

    private Transform playerTransform;

    private float distanceLevelHasReached = 0.0f;

    private float levelTileLength = 100.0f;

    private int numberOfLevelTilesLoaded = 2;

    private float levelTileLoadingDistance = 110.0f;

    private int lastLevelTileIndex = 0;
    
    private List<GameObject> currentlySpawnedLevelTiles;
    
    // Before the initial frame update this method is called 
    private void Start()
    {
    
        // Loads the initial level tiles
        currentlySpawnedLevelTiles = new List<GameObject>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        for (int i = 0; i < numberOfLevelTilesLoaded; i++)
        {
            SpawnLevelTile();
        }
       
    }

    // Update is called once per frame
    private void Update()
    {
        // On every frame a level tile is created depending on if it meets the following condition. The following     
        // conditional statement checks if it is appropriate to load another level tile
        if (playerTransform.position.z - levelTileLoadingDistance > 
            (distanceLevelHasReached - numberOfLevelTilesLoaded * levelTileLength))
        {
            SpawnLevelTile();
            DeleteLevelTile();
        }
    }

    // Spawns the level tiles
    private void SpawnLevelTile()
    {
        // Gets the level tile
        GameObject gameObject;
        gameObject = Instantiate(tilePrefabs[lastLevelTileIndex]) as GameObject;
        gameObject.transform.SetParent(transform);

        // Sets up the tile to load next time
        if (lastLevelTileIndex == 0)
        {
            lastLevelTileIndex = 1;
        }
        else
        {
            lastLevelTileIndex = 0;
        }
        
        // Position the level tiles
        gameObject.transform.position = Vector3.forward * distanceLevelHasReached;
        distanceLevelHasReached += levelTileLength;
        currentlySpawnedLevelTiles.Add(gameObject);
    }

    // Destroy the level tile if the character has passed it
    public void DeleteLevelTile()
    {
        Destroy(currentlySpawnedLevelTiles[0]);
        currentlySpawnedLevelTiles.RemoveAt(0);

    }
}
