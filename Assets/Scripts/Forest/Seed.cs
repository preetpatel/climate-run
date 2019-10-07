using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Represents a collectable seed
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

    // When the player collides with the seed trigger the Collected animation and increment seeds grabbed
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GetComponent<Collider>().enabled = false;
            ForestLevelManager.Instance.getSeeds();
            animator.SetTrigger("Collected");
        }
    }
}
