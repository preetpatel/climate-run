using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{

    public Button musicButton;
    public Button sfxButton;
    
    public static bool isMusicOn = true;
    public static bool isSfxOn = true;

    public static readonly string MUSICKEY = "music";
    public static readonly string SFXKEY = "sfx";

    public void Awake()
    {
        AudioSource[] audios = GameObject.FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in audios)
        {
            if(audio.CompareTag("Music"))
            {
                isMusicOn = !audio.isPlaying;
                onMusicPress();
            }
            
            if(audio.CompareTag("SFX"))
            {
                isSfxOn = !isSfxOn;
                onSFXPress();
            }
        }

    }

    public void onMusicPress()
    {
        isMusicOn = buttonPressed(musicButton, isMusicOn);

        AudioSource[] audios = GameObject.FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in audios)
        {
            if (audio.CompareTag("Music"))
            {
                if (isMusicOn)  
                {
                    if (!audio.isPlaying)
                    {
                        audio.Play();
                    }
                    
                } else
                {
                    audio.Pause();
                }

                SaveState.saveMusicSettings(isMusicOn.ToString(), MUSICKEY);
            }
        }
        
    }

    public void onSFXPress()
    {
        isSfxOn = buttonPressed(sfxButton, isSfxOn);

        GameObject sfx = GameObject.FindGameObjectWithTag("SFX");
        if (isSfxOn)
        {
            isSfxOn = true;
        }
        else
        {
            isSfxOn = false;
        }
        SaveState.saveMusicSettings(isSfxOn.ToString(), SFXKEY);
    }

    public void goBackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private bool buttonPressed(Button buttonPressed, bool isButtonOn)
    {
        Text text = buttonPressed.GetComponentInChildren<Text>();
        Image image = buttonPressed.image;
        if (isButtonOn)
        {
            buttonPressed.image.color = Color.red;
            text.text = "Off";
            isButtonOn = false;
        }
        else
        {
            buttonPressed.image.color = Color.green;
            text.text = "On";
            isButtonOn = true;
        }
        return isButtonOn;
    }
}
