using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garbage : MonoBehaviour
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
        //make sure to spawn garbage at the correct location
        animator.speed = playerMotor.speed / playerMotor.originalSpeed;
        animator.SetTrigger("Spawn");
    }

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        //Collect the garbage when player hit them
        if (other.tag == "Player")
        {
            BeachLevelManager.Instance.getGarbage();
            animator.SetTrigger("Collected");
        }
    }

    public void OnTriggerThrow()
    {
        //throw the garbage
        animator.SetTrigger("Thrown");
    }
}
