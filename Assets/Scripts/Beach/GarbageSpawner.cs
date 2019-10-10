using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageSpawner : MonoBehaviour
{
    public float chanceToSpawn = 0.5f;
    public bool forceSpawnAll = false;
    public bool isTrashObtacle = false;
    public static float garbageMultiplier = 1.0f;

    private GameObject[] garbage;

    private void Awake()
    {
        garbage = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            garbage[i] = transform.GetChild(i).gameObject;
        }
        OnDisable();
    }

    private void OnEnable()
    {
        float chance;
        if (isTrashObtacle)
        {
            chance = garbageMultiplier;
        } else
        {
            chance = chanceToSpawn;
        }
        if (Random.Range(0.0f, 1.0f) > chance)
        {
            return;
        }
        
        if (forceSpawnAll)
        {
            for (int i = 0; i < garbage.Length; i++)
                garbage[i].SetActive(true);
        } else
        {
            for (int i = 0; i < garbage.Length; i++)
            {
                garbage[i].SetActive(true);
            }
        }
    }

    public void OnDisable()
    {
        foreach (GameObject go in garbage)
            go.SetActive(false);
    }
}
