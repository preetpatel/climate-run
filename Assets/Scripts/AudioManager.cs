<<<<<<< HEAD
﻿using System;
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
=======
﻿using System;
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
>>>>>>> 83bad4e0ccbbcd260de4339059b76ac78d288991
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
<<<<<<< HEAD
        }

        
=======
        }

        
>>>>>>> 83bad4e0ccbbcd260de4339059b76ac78d288991
        foreach(Sound sound in sounds)
        {
           sound.source =  gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
<<<<<<< HEAD
        }
    }

    // Update is called once per frame
    public void Play(string name)
    {
       Sound s = Array.Find(sounds, sound => sound.name == name);
=======
        }
    }

    // Update is called once per frame
    public void Play(string name)
    {
       Sound s = Array.Find(sounds, sound => sound.name == name);
>>>>>>> 83bad4e0ccbbcd260de4339059b76ac78d288991
        if (s == null)
        {
            Debug.Log("Sound not found: " + name);
            return;
<<<<<<< HEAD
        }
        s.source.Play();
    }
}
=======
        }
        s.source.Play();
    }
}
>>>>>>> 83bad4e0ccbbcd260de4339059b76ac78d288991
