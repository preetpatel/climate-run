using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DontDestroy : MonoBehaviour
{
    public static bool isSfxOn = true;
    public AudioSource buttonSound;
    public AudioClip buttonSoundClip;

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
        addButtonListeners();
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
}
