using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class PlayerFootsteps : MonoBehaviour
{
    private EventInstance playerFootsteps;
    private Rigidbody rb;

    private void Awake()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
    }

    private void Start()
    {
        playerFootsteps = AudioManager.Instance.CreateInstance(FMODEvents.Instance.playerFootsteps);
    }
}
