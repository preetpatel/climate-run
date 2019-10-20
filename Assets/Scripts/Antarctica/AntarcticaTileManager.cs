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
        if ((playerTransform.position.z - safeZone) > (spawnZ - amnLevelsOnScreen * levelLength))
        {
            SpawnLevel();
        }
    }

    private void SpawnLevel(int prefabIndex = -1)
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

}
