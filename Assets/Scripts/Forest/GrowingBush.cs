using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingBush : MonoBehaviour
{
    private Animator animator;
    
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void Trigger()
    {
        Debug.Log("triggering a bush");
        animator.SetTrigger("Grow");
    }
}
