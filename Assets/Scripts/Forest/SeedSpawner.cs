using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedSpawner : MonoBehaviour
{
    public int maxCoin = 5;
    public float chanceToSpawn = 0.5f;
    public bool forceSpawnAll = false;

    private GameObject[] coins;

    // Initialises coin/seed object
    private void Awake()
    {
        coins = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            coins[i] = transform.GetChild(i).gameObject;
        }
        OnDisable();
    }

    // Randomly spawns the seeds
    private void OnEnable()
    {
        // Randomly decides whether to spawn seeds
        if (Random.Range(0.0f, 1.0f) > chanceToSpawn)
        {
            return;
        }
        if (forceSpawnAll)
        {
            for (int i = 0; i < maxCoin; i++)
                coins[i].SetActive(true);
        } else
        {
            // Randomly spawns a select number of seeds
            int r = Random.Range(0, maxCoin);
            for (int i = 0; i < r; i++)
            {
                coins[i].SetActive(true);
            }
        }
    }

    public void OnDisable()
    {
        foreach (GameObject go in coins)
            go.SetActive(false);
    }
}
