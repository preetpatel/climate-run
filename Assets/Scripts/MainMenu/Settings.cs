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
    
    public static bool? isMusicOn = null;
    public static bool? isSfxOn = null;

    public static string MUSICKEY = "music";
    public static string SFXKEY = "sfx";

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
        isMusicOn = buttonPressed(musicButton, isMusicOn.Value);

        AudioSource[] audios = GameObject.FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in audios)
        {
            if (audio.CompareTag("Music"))
            {
                if (isMusicOn.Value)  
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
        isSfxOn = buttonPressed(sfxButton, isSfxOn.Value);

        GameObject sfx = GameObject.FindGameObjectWithTag("SFX");
        if (isSfxOn.Value)
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
