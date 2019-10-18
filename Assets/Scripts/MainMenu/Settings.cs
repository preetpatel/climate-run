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
    
    private bool isMusicOn = true;
    private bool isSfxOn = true;

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
                    audio.Play();
                } else
                {
                    audio.Pause();
                }
                
            }
        }
        
    }

    public void onSFXPress()
    {
        isSfxOn = buttonPressed(sfxButton, isSfxOn);
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
