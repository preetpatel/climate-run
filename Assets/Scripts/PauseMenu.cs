using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    public static bool isPaused;
    public GameObject pauseMenuUI;
    public GameObject pauseButtonUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                PressResumeButtonHandler();
            else
                PressPauseButtonHandler();
        }
      

        
    }

    public void PressPauseButtonHandler()
    {
        Debug.Log("Hi");
        pauseMenuUI.SetActive(true);
        pauseButtonUI.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;

    }

    public void PressResumeButtonHandler()
    {
        pauseMenuUI.SetActive(false);
        pauseButtonUI.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;
    }
}
