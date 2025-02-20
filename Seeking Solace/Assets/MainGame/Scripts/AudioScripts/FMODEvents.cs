using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Player SFX")]
    [field: SerializeField] public EventReference playerFootsteps { get; private set; }

    [field: Header("Object SFX")]
    [field: SerializeField] public EventReference itemPickedUp { get; private set; }
    [field: SerializeField] public EventReference doorOpen { get; private set; }
    [field: SerializeField] public EventReference doorClose { get; private set; }

    [field: Header("Ambience SFX")]
    [field: SerializeField] public EventReference droneAmbience { get; private set; }

    public static FMODEvents Instance { get; private set; }

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
