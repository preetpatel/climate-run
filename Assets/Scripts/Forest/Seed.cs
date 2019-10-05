using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            ForestLevelManager.Instance.getSeeds();
            animator.SetTrigger("Collected");
            Destroy(this.gameObject, 1.5f);
        }
    }
}
