using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        GetComponent<Collider>().enabled = true;
    }

    private void OnEnable()
    {
        animator.SetTrigger("Spawn");
    }

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.tag == "Player")
        {
            GetComponent<Collider>().enabled = false;
            ForestLevelManager.Instance.getSeeds();
            animator.SetTrigger("Collected");
        }
    }
}
