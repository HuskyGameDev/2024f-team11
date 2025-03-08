using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using FMODUnity;
using System;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    private EventInstance mainMusic;
    private EventInstance GUIButtonPress;
    private List<EventInstance> eventInstances;
    private List<StudioEventEmitter> eventEmitters;
    FMOD.Studio.Bus masterBus;
    FMOD.Studio.Bus sfxBus;
    FMOD.Studio.Bus musicBus;
    FMOD.Studio.Bus ambienceBus;

    public static AudioManager Instance { get; private set; }

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

        eventInstances = new List<EventInstance>();
        eventEmitters = new List<StudioEventEmitter>();

        masterBus = FMODUnity.RuntimeManager.GetBus("bus:/");
        sfxBus = FMODUnity.RuntimeManager.GetBus("bus:/SFX");
        musicBus = FMODUnity.RuntimeManager.GetBus("bus:/Music");
        ambienceBus = FMODUnity.RuntimeManager.GetBus("bus:/Ambience");

        mainMusic = AudioManager.Instance.CreateInstance(FMODEvents.Instance.mainMusic);
        GUIButtonPress = AudioManager.Instance.CreateInstance(FMODEvents.Instance.GUIButtonPress);
        mainMusic.start();

    }

    private void OnEnable()
    {
        GUIManager.OnMasterSliderChanged += UpdateMasterVolume;
        GUIManager.OnMusicSliderChanged += UpdateMusicVolume;
        GUIManager.OnAmbienceSliderChanged += UpdateAmbienceVolume;
        GUIManager.OnSFXSliderChanged += UpdateSFXVolume;
        GUIManager.OnButtonPressed += PlayButtonPressedSound;
    }

    private void OnDisable()
    {
        GUIManager.OnMasterSliderChanged -= UpdateMasterVolume;
        GUIManager.OnMusicSliderChanged -= UpdateMusicVolume;
        GUIManager.OnAmbienceSliderChanged -= UpdateAmbienceVolume;
        GUIManager.OnSFXSliderChanged -= UpdateSFXVolume;
        GUIManager.OnButtonPressed -= PlayButtonPressedSound;
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public bool IsPlaying(FMOD.Studio.EventInstance instance)
    {
        FMOD.Studio.PLAYBACK_STATE state;
        instance.getPlaybackState(out state);
        return state != FMOD.Studio.PLAYBACK_STATE.STOPPED;
    }

    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    public StudioEventEmitter InitializeEventEmitter(EventReference eventReference, GameObject emitterGameObject)
    {
        StudioEventEmitter emitter = emitterGameObject.GetComponent<StudioEventEmitter>();
        emitter.EventReference = eventReference;
        eventEmitters.Add(emitter);
        return emitter;
    }

    private void CleanUp()
    {
        // stop and release any created instances
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }

        // stop all of the event emitters, because if we don't they may hang around in other scenes
        foreach (StudioEventEmitter emitter in eventEmitters)
        {
            emitter.Stop();
        }
    }

    void UpdateMasterVolume(float value)
    {
        masterBus.setVolume(value);
    }

    void UpdateMusicVolume(float value)
    {
        musicBus.setVolume(value);
    }

    void UpdateSFXVolume(float value)
    {
        sfxBus.setVolume(value);
    }

    void UpdateAmbienceVolume(float value)
    {
        ambienceBus.setVolume(value);
    }

    void PlayButtonPressedSound()
    {
        GUIButtonPress.start();
    }

    private void OnDestroy()
    {
        CleanUp();
    }
}
