﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garbage : MonoBehaviour
{

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
 
        if(other.tag == "Player")
        {
            LevelManagerBeach.Instance.getGarbage();
            anim.SetTrigger("Collected");
            Destroy(this.gameObject, 1.5f);
        }
        
    }
}