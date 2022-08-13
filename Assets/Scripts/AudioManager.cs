using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] sounds;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
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
        }

        playBG();
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

    public void playBG()
    {
        Sound s = System.Array.Find(sounds, s => s.name == "BGMusic");

        if (s == null)
        {
            Debug.Log("sound name not found: " + name);
            return;
        }

        s.source.Play();
    }
}
