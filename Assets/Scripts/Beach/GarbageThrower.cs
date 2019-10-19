using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageThrower : MonoBehaviour
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
    
    public void OnTriggerThrow()
    {
        animator.SetTrigger("Throw");
    }
}
