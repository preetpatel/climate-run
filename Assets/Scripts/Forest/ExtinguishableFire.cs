using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Author: Marc Burgess
 */
public class ExtinguishableFire : MonoBehaviour
{
    private bool transitioning = false;

    public void Trigger()
    {
        transitioning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (transitioning)
        {
            Color currentColor = GetComponent<ParticleSystem>().main.startColor.color;
            float newR = currentColor.r - 0.02f;
            float newG = currentColor.g - 0.02f;
            float newB = currentColor.b - 0.02f;
            float newA = currentColor.a - 0.02f;

            if (currentColor.r <= 0.0f)
            {
                gameObject.SetActive(false);
                transitioning = false;
            }
            else
            {
                if (newR < 0.0f)
                    newR = 0.0f;
                if (newG < 0.0f)
                    newG = 0.0f;
                if (newB < 0.0f)
                    newB = 0.0f;
                if (newA < 0.0f)
                    newA = 0.0f;

                Color newColor = new Color(newR, newG, newB, newA);
                ParticleSystem.MinMaxGradient newGradient = new ParticleSystem.MinMaxGradient(newColor);

                ParticleSystem.MainModule ma = gameObject.GetComponent<ParticleSystem>().main;
                ma.startColor = newGradient;
            }
        }
    }
}
