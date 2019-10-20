using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Music : MonoBehaviour
{
    public static bool isSfxOn = true;
    public AudioSource buttonSound;
    public AudioClip buttonSoundClip;

    public AudioClip mainMenuMusic;

    public AudioClip forestLevelMusic;

    public AudioClip beachLevelMusic;

    public AudioSource music;

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Music");
        if(objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
        
    }

    void Start()
    {
        if (buttonSound.CompareTag("SFX")) {
            addButtonListeners();
            SceneManager.sceneLoaded += OnSceneLoaded;

        }
    }
    private void OnSceneLoaded(Scene aScene, LoadSceneMode aMode)
    {

        playMainMenuMusic(aScene);

        if (buttonSound.CompareTag("SFX"))
        {
            addButtonListeners();
        }
    }

    private void addButtonListeners()
    {
        Button[] buttons = GameObject.FindObjectsOfType<Button>();
        foreach (Button btn in buttons)
        {
            btn.onClick.AddListener(() => playSound());
        }
    }
    public void playSound()
    {

        if (isSfxOn)
        {
            buttonSound.PlayOneShot(buttonSoundClip);
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
}
