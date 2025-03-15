using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: Header("SFX")]
    [field: SerializeField] public EventReference playerFootsteps { get; private set; }
    [field: SerializeField] public EventReference itemPickedUp { get; private set; }
    [field: SerializeField] public EventReference doorOpen { get; private set; }
    [field: SerializeField] public EventReference doorClose { get; private set; }
    [field: SerializeField] public EventReference wardrobeOpen { get; private set; }
    [field: SerializeField] public EventReference wardrobeClose { get; private set; }
    [field: SerializeField] public EventReference drawerOpen { get; private set; }
    [field: SerializeField] public EventReference drawerClose { get; private set; }
    [field: SerializeField] public EventReference pickUpBS { get; private set; }
    [field: SerializeField] public EventReference dropBS { get; private set; }
    [field: SerializeField] public EventReference GUIButtonPress { get; private set; }
    [field: SerializeField] public EventReference WakeUp { get; private set; }

    [field: Header("Music")]
    [field: SerializeField] public EventReference mainMusic { get; private set; }


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
