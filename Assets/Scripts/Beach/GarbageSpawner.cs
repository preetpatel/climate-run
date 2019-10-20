using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageSpawner : MonoBehaviour
{
    public float chanceToSpawn = 0.5f;

    private GameObject[] garbageAndThrower;

    private Transform playerTransform;

    private bool thrown = false;

    private void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        garbageAndThrower = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            garbageAndThrower[i] = transform.GetChild(i).gameObject;
        }
        OnDisable();
    }

    private void Update() 
    {
        if (garbageAndThrower[0].activeInHierarchy && transform.position.z < playerTransform.position.z + 35 && !thrown)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                garbageAndThrower[i].SendMessage("OnTriggerThrow");
            }
            thrown = true;
        }
    }

    private void OnEnable()
    {
        thrown = false;
        if (Random.Range(0.0f, 1.0f) > chanceToSpawn)
        {
            return;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            garbageAndThrower[i].SetActive(true);
        }
    }

    public void OnDisable()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            garbageAndThrower[i].SetActive(false);
        }
    }
}
