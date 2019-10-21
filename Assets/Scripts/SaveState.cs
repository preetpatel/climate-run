using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveState : MonoBehaviour
{
    public static bool saveHighScore(int score, string key)
    {
        if (score > PlayerPrefs.GetInt(key))
        {
            PlayerPrefs.SetInt(key, score);
            return true;
        }

        return false;
    }

    public static void saveMusicSettings(string musicState, string key)
    {
        PlayerPrefs.SetString(key, musicState);
    }
}
