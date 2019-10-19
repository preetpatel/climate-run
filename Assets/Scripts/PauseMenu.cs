using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static bool isPaused;
    public GameObject pauseMenuUI;
    public GameObject pauseButtonUI;

    private AudioSource audioPlayer;

    private void Start()
    {
        Time.timeScale = 1f;
        AudioSource[] audios = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in audios)
        {
            if (audio.CompareTag("Music"))
            {
                audioPlayer = audio;
            }
        }
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
        audioPlayer.Pause();
    }

    public void PressResumeButtonHandler()
    {
        pauseMenuUI.SetActive(false);
        pauseButtonUI.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;
        audioPlayer.Play();
    }

    public void PressMenuButtonHandler()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene("MainMenu");
    }
}