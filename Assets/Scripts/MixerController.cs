using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MixerController : MonoBehaviour
{
    [SerializeField]
    AudioMixer audioMixer;

    Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();

        if (audioMixer.name == "MusicMixer")
            slider.value = AudioManager.instance.musicVolume;
        else if (audioMixer.name == "SoundMixer")
            slider.value = AudioManager.instance.soundVolume;

        setVolume(slider.value);
    }

    public void setVolume(float value)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);

        if (audioMixer.name == "MusicMixer")
            AudioManager.instance.musicVolume = slider.value;
        else if (audioMixer.name == "SoundMixer")
            AudioManager.instance.soundVolume = slider.value;
    }

}
