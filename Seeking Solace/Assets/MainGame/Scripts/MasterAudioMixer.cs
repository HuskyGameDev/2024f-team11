using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterAudioMixer : MonoBehaviour
{
    public static MasterAudioMixer Instance { get; private set; }

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
}
