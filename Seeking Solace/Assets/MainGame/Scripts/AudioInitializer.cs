using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioInitializer : MonoBehaviour
{
    [SerializeField] Sound[] sounds;
    // Start is called before the first frame update
    void Awake()
    {

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            if (s.audioMixerGroup) { s.source.outputAudioMixerGroup = s.audioMixerGroup; }
            else s.source.outputAudioMixerGroup = AudioManager.Instance.getMasterMixer().FindMatchingGroups("SFX")[0];

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;
            s.source.spatialBlend = s.spatialBlend;

            if (s.source.playOnAwake)
            {
                s.source.Play();
                        }
        }
    }

    public Sound RetrieveSound(string name)
    {
        //Debug.Log(Array.Find(sounds, sound => sound.name == name).name);
        return Array.Find(sounds, sound => sound.name == name);
    }
}
