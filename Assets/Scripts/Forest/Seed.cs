using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        animator.SetTrigger("Spawn");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            ForestLevelManager.Instance.getSeeds();
            animator.SetTrigger("Collected");
        }
    }
}
