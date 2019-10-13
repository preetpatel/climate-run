using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static bool isPaused;
    public GameObject pauseMenuUI;
    public GameObject pauseButtonUI;

    private void Start()
    {
        Time.timeScale = 1f;
    }

    // For enabling pause menu on PC using the escape key
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

    public void PressMenuButtonHandler()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene("MainMenu");
    }
}