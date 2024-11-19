using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] AudioMixer masterMixer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    //public void PlayMusic(string name)
    //{
    //    Sound s = Array.Find(sounds, sound => sound.name == name);
    //    s.source.Play();
    //    s.source.loop = true;
    //    //currentSong = name;
    //}

    //public void StopMusic(string name)
    //{
    //    Sound s = Array.Find(musicSounds, sound => sound.name == name);
    //    s.source.Stop();
    //}

    //public void StopCurrentMusic()
    //{
    //    Sound s = Array.Find(musicSounds, sound => sound.name == currentSong);
    //    s.source.Stop();
    //}

    public void PlaySound(GameObject source, string name)
    {
        if (source.GetComponent<AudioInitializer>() != null)
        {
            Sound s = source.GetComponent<AudioInitializer>().RetrieveSound(name);
            if (s != null)
                s.source.Play();
        }
    }

    //public void PlaySoundOneShot(string name)
    //{
    //    Sound s = Array.Find(sfxSounds, sound => sound.name == name);
    //    if (s != null)
    //        s.source.PlayOneShot(s.clip);
    //}

    public void StopSound(GameObject source, string name)
    {
        if (source.GetComponent<AudioInitializer>() != null)
        {
            Sound s = source.GetComponent<AudioInitializer>().RetrieveSound(name);
            if (s != null)
                s.source.Stop();
        }
    }

    //public void ChangeMainVolume()
    //{
    //    mixer.SetFloat("MasterVolume", Mathf.Log10(mainSlider.value) * 20);
    //    mainText.text = Mathf.Round(Mathf.Clamp(100 * mainSlider.value, 0f, 100f)).ToString();
    //}
    //public void ChangeSFXVolume()
    //{
    //    mixer.SetFloat("SFXVolume", Mathf.Log10(SFXSlider.value) * 20);
    //    SFXText.text = Mathf.Round(Mathf.Clamp(100 * SFXSlider.value, 0f, 100f)).ToString();
    //}

    //public void ChangeMusicVolume()
    //{
    //    mixer.SetFloat("MusicVolume", Mathf.Log10(musicSlider.value) * 20);
    //    musicText.text = Mathf.Round(Mathf.Clamp(100 * musicSlider.value, 0f, 100f)).ToString();
    //}

    //public void LoadData(S_O_Saving saver)
    //{
    //    mainSlider.value = saver.audioData.MasterVolume;
    //    //Debug.Log(mainSlider.value);
    //    mainText.text = Mathf.Round(Mathf.Clamp(100 * mainSlider.value, 0f, 100f)).ToString();
    //    ChangeMainVolume();

    //    musicSlider.value = saver.audioData.MusicVolume;
    //    musicText.text = Mathf.Round(Mathf.Clamp(100 * musicSlider.value, 0f, 100f)).ToString();
    //    ChangeMusicVolume();

    //    SFXSlider.value = saver.audioData.SFXVolume;
    //    SFXText.text = Mathf.Round(Mathf.Clamp(100 * SFXSlider.value, 0f, 100f)).ToString();
    //    ChangeSFXVolume();
    //}

    //public void SaveData(S_O_Saving saver)
    //{
    //    saver.audioData.MasterVolume = mainSlider.value;
    //    saver.audioData.MusicVolume = musicSlider.value;
    //    saver.audioData.SFXVolume = SFXSlider.value;
    //}

    public AudioMixer getMasterMixer() { return masterMixer; }
}
