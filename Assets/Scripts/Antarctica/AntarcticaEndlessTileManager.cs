using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class AntarcticaEndlessTileManager : MonoBehaviour
{
    public GameObject[] tilePrefabs;

    private Transform playerTransform;
   

    private float spawnZ = 0.0f;

    private float levelLength = 118.0f;

    private int amnLevelsOnScreen = 3;

    private float safeZone = 110.0f;

    private int lastPrefabIndex = 0;
    
    private static int numberOfLevelTiles = 0;
    
    private List<GameObject> activeLevels;
    // Start is called before the first frame update
    private void Start()
    {
        activeLevels = new List<GameObject>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        numberOfLevelTiles = 0;
        for (int i = 0; i < amnLevelsOnScreen; i++)
        {
            SpawnLevel();
        }
       
    }

    // Update is called once per frame
    private void Update()
    {
        if ((playerTransform.position.z - safeZone) > (spawnZ - amnLevelsOnScreen * levelLength))
        {
            SpawnLevel();
            DeleteLevel();
        }
    }

    private void SpawnLevel(int prefabIndex = -1)
    {
        GameObject go;
        int spawnLevelTileIndex = 0;
        int[] levelTiles;

        if (numberOfLevelTiles > 0)
        {
            levelTiles = new int[] {0, 1, 2, 3};
        }
        else
        {
            levelTiles = new int[] {0, 2, 3};
        }

        Vector3 position = new Vector3(-1000,-10,100);

        spawnLevelTileIndex = Random.Range(0, levelTiles.Length);
        GameObject newTile = tilePrefabs[levelTiles[spawnLevelTileIndex]];
            
        go = Instantiate(newTile,position,newTile.transform.rotation) as GameObject;
        go.transform.SetParent(transform);
        
        spawnZ += levelLength;
        activeLevels.Add(go);
        numberOfLevelTiles++;
    }

    public void DeleteLevel()
    {
        Destroy(activeLevels[0]);
        activeLevels.RemoveAt(0);

    }
 }
