using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// Audio Manager class for managing audio for each scene
// Written by - Preet Patel

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        foreach(Sound sound in sounds)
        {
           sound.source =  gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }

    // Update is called once per frame
    public void Play(string name)
    {
       Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Sound not found: " + name);
            return;
        }
        s.source.Play();
    }
}
