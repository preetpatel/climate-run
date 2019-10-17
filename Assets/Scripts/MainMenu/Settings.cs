using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{

    public Button musicButton;
    public Button sfxButton;
    
    private bool isMusicPressed = false;
    private bool isSfxPressed = false;

    public void onMusicPress()
    {
        isMusicPressed = buttonPressed(musicButton, isMusicPressed);
    }

    public void onSFXPress()
    {
        isSfxPressed = buttonPressed(sfxButton, isSfxPressed);
    }

    private bool buttonPressed(Button buttonPressed, bool buttonStatus)
    {
        Text text = buttonPressed.GetComponentInChildren<Text>();
        Image image = buttonPressed.image;
        if (buttonStatus)
        {
            buttonPressed.image.color = Color.green;
            text.text = "On";
            buttonStatus = false;
        }
        else
        {
            buttonPressed.image.color = Color.red;
            text.text = "Off";
            buttonStatus = true;
        }
        return buttonStatus;
    }
}
