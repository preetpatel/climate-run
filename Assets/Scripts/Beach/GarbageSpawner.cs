using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageSpawner : MonoBehaviour
{
    public float chanceToSpawn = 0.5f;
    public bool forceSpawnAll = false;

    private GameObject[] garbage;

    private Transform playerTransform;

    private void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        garbage = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            garbage[i] = transform.GetChild(i).gameObject;
        }
        OnDisable();
    }

    private void Update() 
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (garbage[i].activeInHierarchy && garbage[i].transform.position.z < playerTransform.position.z + 25)
            {
                garbage[i].SendMessage("OnTriggerThrow");
            }
        }
    }

    private void OnEnable()
    {
        if (Random.Range(0.0f, 1.0f) > chanceToSpawn)
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
