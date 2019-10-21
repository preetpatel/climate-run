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
        if (Settings.isMusicOn.Value)
        {
            AudioSource[] audios = FindObjectsOfType<AudioSource>();
            foreach (AudioSource audio in audios)
            {
                if (audio.CompareTag("Music"))
                {
                    audioPlayer = audio;
                }
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
        if (Settings.isMusicOn.Value)
            audioPlayer.Pause();
    }

    public void PressResumeButtonHandler()
    {
        pauseMenuUI.SetActive(false);
        pauseButtonUI.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;
        if (Settings.isMusicOn.Value)
            audioPlayer.Play();
    }

    public void PressMenuButtonHandler()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void PressSkipButtonHandler()
    {
        Time.timeScale = 1f;
        isPaused = false;
        pauseMenuUI.SetActive(false);
        pauseButtonUI.SetActive(true);

        Scene gameScene = SceneManager.GetActiveScene();
        if (gameScene.name.Equals("Forest"))
        {
            ForestLevelManager LevelManager;
            LevelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<ForestLevelManager>();
            //LevelManager.SkipLevel();
        }
        else if (gameScene.name.Equals("Beach"))
        {
            BeachLevelManager LevelManager;
            LevelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<BeachLevelManager>();
            LevelManager.SkipLevel();
        }
        else if (gameScene.name.Equals("Antarctica"))
        {
            AntarcticaLevelManager LevelManager;
            LevelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<AntarcticaLevelManager>();
            LevelManager.SkipLevel();
        }

    }
}