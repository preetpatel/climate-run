using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Author: Marc Burgess
 */
public class FireTruckAction : MonoBehaviour
{
    private bool triggeredAlready = false;
    private bool transitioning = false;
    private int framesSinceTrigger = 0;
    private Dictionary<ExtinguishableFire, bool> extinguishableFireParticles;

    internal void doWaterSpray()
    {
        if (!triggeredAlready)
        {
            Debug.Log("Triggering the firetruck");
            triggeredAlready = true;
            Time.timeScale = 0.5f;
            transitioning = true;

            extinguishableFireParticles = new Dictionary<ExtinguishableFire, bool>();
            ExtinguishableFire[] fireParticles = GetComponentsInChildren<ExtinguishableFire>();
            for (int i = 0; i < fireParticles.Length; i++)
            {
                extinguishableFireParticles.Add(fireParticles[i], false);
            }
        }
    }

    void Update()
    {
        if(transitioning)
        {
            framesSinceTrigger++;

            foreach (KeyValuePair<ExtinguishableFire, bool> particleHasDespawned in extinguishableFireParticles)
            {
                ExtinguishableFire particle = particleHasDespawned.Key;
                if(!particleHasDespawned.Value)
                {
                    extinguishableFireParticles[particle] = true;
                    particle.Trigger();

                    break;
                }
            }

            if (framesSinceTrigger > 240)
            {
                transitioning = false;
                Time.timeScale = 1.0f;
            }
        }
    }
}
