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
    private int framesSinceExtinguish = 0;
    private Dictionary<SplashFireCombo, bool> extinguishableFireParticles;

    internal void doWaterSpray()
    {
        if (!triggeredAlready)
        {
            triggeredAlready = true;
            Time.timeScale = 0.3f;
            transitioning = true;

            extinguishableFireParticles = new Dictionary<SplashFireCombo, bool>();
            SplashFireCombo[] fireParticles = GetComponentsInChildren<SplashFireCombo>();
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
            if (framesSinceTrigger > 160)
            {
                transitioning = false;
                Time.timeScale = 1.0f;
                return;
            }

            framesSinceExtinguish++;

            if (framesSinceExtinguish >= 15)
            {
                framesSinceExtinguish = 0;

                foreach (KeyValuePair<SplashFireCombo, bool> particleHasDespawned in extinguishableFireParticles)
                {
                    SplashFireCombo particle = particleHasDespawned.Key;
                    if (!particleHasDespawned.Value)
                    {
                        extinguishableFireParticles[particle] = true;
                        particle.Trigger();

                        break;
                    }
                }
            }

            framesSinceTrigger++;
        }
    }
}
