using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTruckAction : MonoBehaviour
{
    private bool triggeredAlready = false;

    internal void doWaterSpray()
    {
        if (!triggeredAlready)
        {
            triggeredAlready = true;
            Time.timeScale = 0.5f;
        }
        
    }
}
