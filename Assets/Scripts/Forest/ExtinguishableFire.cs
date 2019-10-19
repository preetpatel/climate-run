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
        Debug.Log("Triggering a fire");
    }

    // Update is called once per frame
    void Update()
    {
        if (transitioning)
        {
            Color currentColor = GetComponent<ParticleSystem>().startColor;
            Debug.Log(currentColor.a);
            float newA = currentColor.a - 10;

            if (currentColor.a <= 0.0f)
            {
                gameObject.SetActive(false);
                transitioning = false;
            }
            else
            {
                if (newA <= 0.0f)
                    newA = 0.0f;

                gameObject.GetComponent<ParticleSystem>().startColor =
                    new Color(currentColor.r, currentColor.g, currentColor.b, newA);
            }
        }
    }
}
