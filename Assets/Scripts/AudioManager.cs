using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioMixerGroup musicGroup, soundGroup;
    public float musicVolume, soundVolume;

    public Sound[] sounds;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            musicVolume = soundVolume = 1f;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (var s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();

            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

            if (s.name == "BGMusic") 
                s.source.outputAudioMixerGroup = musicGroup;
            else 
                s.source.outputAudioMixerGroup = soundGroup;
        }

        play("BGMusic");
    }

    public void play(string name)
    {
        Sound s = System.Array.Find(sounds, s => s.name == name);

        if (s == null)
        {
            Debug.Log("sound name not found: " + name);
            return;
        }

        s.source.Play();
    }
}
