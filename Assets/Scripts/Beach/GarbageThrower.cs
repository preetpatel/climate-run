using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageThrower : MonoBehaviour
{

    private Animator animator;
    private PlayerMotor playerMotor;

    private void Awake() 
    {
        playerMotor = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMotor>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        animator.speed = playerMotor.speed / playerMotor.originalSpeed;
        animator.SetTrigger("Spawn");
    }
    
    public void OnTriggerThrow()
    {
        animator.SetTrigger("Throw");
    }
}
