using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garbage : MonoBehaviour
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

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.tag == "Player")
        {
            BeachLevelManager.Instance.getGarbage();
            animator.SetTrigger("Collected");
        }
    }

    public void OnTriggerThrow()
    {
        animator.SetTrigger("Thrown");
    }
}
