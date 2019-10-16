using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashSpawner : MonoBehaviour
{
    public static float garbageMultiplier = 1.0f;

    private GameObject trash;

    private Transform playerTransform;

    private void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        // Should only be one trash obstacle
        trash = transform.GetChild(0).gameObject;
        OnDisable();
    }

    private void Update() 
    {
        // Nothing atm
    }

    private void OnEnable()
    {
        if (Random.Range(0.0f, 1.0f) > garbageMultiplier)
        {
            return;
        } else
        {
            trash.SetActive(true);
        }
    }

    public void OnDisable()
    {
        trash.SetActive(false);
    }
}
