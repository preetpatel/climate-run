using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashFireCombo : MonoBehaviour
{
    private bool transitioning = false;

    public void Trigger()
    {
        transitioning = true;
        ExtinguishableFire fire = GetComponentInChildren<ExtinguishableFire>();
        fire.Trigger();

        GameObject splashObj = transform.Find("Splash_Mobile").gameObject;
        if (splashObj != null)
            splashObj.GetComponent<ParticleSystem>().Play();
    }
}
