using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/**
 *  This class is made to handle Music and SFX
 **/
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

        // Check if the values are null (will be if settings not opened)
        if (!Settings.isMusicOn.HasValue || !(Settings.MUSICKEY == null))
        {
            Settings.isMusicOn = true;
            Settings.MUSICKEY = "music";
        } else
        {
            Settings.isMusicOn = Boolean.Parse(PlayerPrefs.GetString(Settings.MUSICKEY));
        }

        if(!Settings.isSfxOn.HasValue || !(Settings.SFXKEY == null))
        {
            Settings.isSfxOn = true;
            Settings.SFXKEY = "sfx";
        } else
        {
            Settings.isSfxOn = Boolean.Parse(PlayerPrefs.GetString(Settings.SFXKEY));
        }

        if(Settings.isMusicOn.Value)
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

    // called every time a new scene is loaded
    private void OnSceneLoaded(Scene aScene, LoadSceneMode aMode)
    {

        playMainMenuMusic(aScene);

        if (sfxPlayer.CompareTag("SFX"))
        {
            addButtonListeners();
        }
    }

    // used to add listeners to all buttons in the scene
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

        if (Settings.isSfxOn.Value)
        {
            sfxPlayer.PlayOneShot(buttonSFX);
        }

    }

    // used to switch music depending on the scene its currently in
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
            if (!music.clip.name.Equals(mainMenuMusic.name) && Settings.isMusicOn.Value)
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
        if(Settings.isSfxOn.Value)
        {
            sfxPlayer.PlayOneShot(gameOverSFX);
        }
    }

    public void playDamage()
    {
        if (Settings.isSfxOn.Value)
        {
            sfxPlayer.PlayOneShot(damageSFX);
        }
    }
}
