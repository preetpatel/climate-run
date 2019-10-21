using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Music : MonoBehaviour
{
    public AudioSource sfxPlayer;
    public AudioSource music;

    // SFX
    public AudioClip buttonSFX;
    public AudioClip gameOverSFX;
    public AudioClip damageSFX;

    // Music
    public AudioClip mainMenuMusic;
    public AudioClip forestLevelMusic;
    public AudioClip beachLevelMusic;
    public AudioClip antarcticaLevelMusic;


    

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("SoundController");
        if(objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        Settings.isMusicOn = Boolean.Parse(PlayerPrefs.GetString(Settings.MUSICKEY));
        Settings.isSfxOn = Boolean.Parse(PlayerPrefs.GetString(Settings.SFXKEY));
        
        if(Settings.isMusicOn)
        {
            music.Play();
        }
    }

    void Start()
    {
        if (sfxPlayer.CompareTag("SFX")) {
            addButtonListeners();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

    }
    private void OnSceneLoaded(Scene aScene, LoadSceneMode aMode)
    {

        playMainMenuMusic(aScene);

        if (sfxPlayer.CompareTag("SFX"))
        {
            addButtonListeners();
        }
    }

    private void addButtonListeners()
    {
        Button[] buttons = Resources.FindObjectsOfTypeAll<Button>();
        foreach (Button btn in buttons)
        {
            btn.onClick.AddListener(() => playSound());
        }
    }
    public void playSound()
    {

        if (Settings.isSfxOn)
        {
            sfxPlayer.PlayOneShot(buttonSFX);
        }

    }

    public void changeMusic(Scene aScene)
    {
        if (!aScene.name.Equals("Settings") && !aScene.name.Equals("MainMenu"))
        {
            if (aScene.name.Equals("Forest"))
            { 
                StartCoroutine(AudioController.FadeIn(music, 0.5f));
                music.clip = forestLevelMusic;
                music.Play();
            } else if (aScene.name.Equals("Beach"))
            {
                StartCoroutine(AudioController.FadeIn(music, 0.5f));
                music.clip = beachLevelMusic;
                music.Play();
            } else if (aScene.name.Equals("Antarctica"))
            {
                StartCoroutine(AudioController.FadeIn(music, 0.5f));
                music.clip = antarcticaLevelMusic;
                music.Play();
            }
        } else
        {
            playMainMenuMusic(aScene);
        }
        
    }

    private void playMainMenuMusic(Scene scene)
    {
        if (scene.name.Equals("Settings") || scene.name.Equals("MainMenu"))
        {
            if (!music.clip.name.Equals(mainMenuMusic.name) && Settings.isMusicOn)
            { 
                StartCoroutine(AudioController.FadeIn(music, 0.5f));
                music.clip = mainMenuMusic;
                music.loop = true;
                music.Play();
            }
        }
    }

    public void playGameOver()
    {
        if(Settings.isSfxOn)
        {
            sfxPlayer.PlayOneShot(gameOverSFX);
        }
    }

    public void playDamage()
    {
        if (Settings.isSfxOn)
        {
            sfxPlayer.PlayOneShot(damageSFX);
        }
    }
}
