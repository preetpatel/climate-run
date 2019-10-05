using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        ForestLevelManager.Instance.getSeeds();
        animator.SetTrigger("Collected");
    }
}
