using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageSpawner : MonoBehaviour
{
    public float chanceToSpawn = 0.5f;

    private GameObject garbage;

    private Transform playerTransform;

    private void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        // Should only be one piece of garbage per script
        garbage = transform.GetChild(0).gameObject;
        OnDisable();
    }

    private void Update() 
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (garbage.activeInHierarchy && garbage.transform.position.z < playerTransform.position.z + 25)
            {
                garbage.SendMessage("OnTriggerThrow");
            }
        }
    }

    private void OnEnable()
    {
        if (Random.Range(0.0f, 1.0f) > chanceToSpawn)
        {
            return;
        }

        garbage.SetActive(true);
    }

    public void OnDisable()
    {
        garbage.SetActive(false);
    }
}
