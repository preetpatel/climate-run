using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntarcticaTileManager : MonoBehaviour
{
    public GameObject[] tilePrefabs;

    private Transform playerTransform;

    private float spawnZ = 0.0f;

    private float levelLength = 105.0f;

    private int amnLevelsOnScreen = 3;

    private float safeZone = 110.0f;

    private int lastPrefabIndex = 0;
    
    private List<GameObject> activeLevels;

    private GameObject player;
    
    private bool isEndless;
    
    private static int numberOfLevelTiles;

    // Start is called before the first frame update
    private void Start()
    {
        activeLevels = new List<GameObject>();
        player = GameObject.FindGameObjectWithTag("Player");
        PlayerMotor playerScript = player.GetComponent<PlayerMotor>();
        isEndless = SceneController.isEndless;
        playerTransform = player.transform;
        numberOfLevelTiles = 0;
        for (int i = 0; i < amnLevelsOnScreen; i++)
        {
            SpawnLevel();
        }
       
    }

    // Update is called once per frame
    private void Update()
    {
        // Spawns level tile and also deletes it
        if ((playerTransform.position.z - safeZone) > (spawnZ - amnLevelsOnScreen * levelLength))
        {
            SpawnLevel();
            DeleteLevel();
        }
    }
    
    
    private void SpawnLevel(int prefabIndex = -1)
    {
        // Select spawn level for current mode
        if (isEndless)
        {
            EndlessSpawnLevel();
        }
        else
        {
            StorySpawnLevel();
        }
    }
    
    public void EndlessSpawnLevel()
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
            levelTiles = new int[] {0};
        }

        Vector3 position = new Vector3(0,0,1);
        position = position * spawnZ;
        spawnLevelTileIndex = Random.Range(0, levelTiles.Length);
        GameObject newTile = tilePrefabs[levelTiles[spawnLevelTileIndex]];
            
        go = Instantiate(newTile,position,newTile.transform.rotation) as GameObject;
        go.transform.SetParent(transform);
        
        spawnZ += levelLength;
        activeLevels.Add(go);
        numberOfLevelTiles++;
        
    }

    public void StorySpawnLevel()
    {
        if (lastPrefabIndex < 5)
        {
            GameObject go;
            go = Instantiate(tilePrefabs[lastPrefabIndex]) as GameObject;
            go.transform.SetParent(transform);

            lastPrefabIndex++;

            go.transform.position = Vector3.forward * spawnZ;
            spawnZ += levelLength;
            activeLevels.Add(go);
        }

    }
    
    public void DeleteLevel()
    {
        if (activeLevels.Count > 3)
        {
            Destroy(activeLevels[0]);
            activeLevels.RemoveAt(0);
        }

    }


}
