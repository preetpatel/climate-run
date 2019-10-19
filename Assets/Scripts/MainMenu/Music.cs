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

    public AudioClip forestLevelMusic;

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
        if(music.CompareTag("Music"))
        {
            if(aScene.name != "Settings" && aScene.name != "MainMenu")
            {
                IEnumerator fadeSound1 = AudioController.FadeOut(music, 0.5f);
                StartCoroutine(fadeSound1);
                if (aScene.name.Equals("Forest"))
                {
                    StartCoroutine(AudioController.FadeIn(music, 2f));
                    music.clip = forestLevelMusic;
                    music.Play();
                }
            }
           
        }
        if(buttonSound.CompareTag("SFX"))
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
        
        if(isSfxOn)
        {
            buttonSound.PlayOneShot(buttonSoundClip);
        }
        
    }

    public IEnumerator wait(float sec)
    {
        yield return new WaitForSeconds(sec);
    }
}
