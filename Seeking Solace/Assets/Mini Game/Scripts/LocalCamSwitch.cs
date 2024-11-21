using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LocalCamSwitch : MonoBehaviour
{
    public CinemachineVirtualCamera newCam;
    public Transform obj;
    public LayerMask player;
    public CamSwitch camManager;

    public float distance;

    bool inRange;


    // Update is called once per frame
    void Update()
    {
        inRange = Physics.CheckSphere(obj.position, distance, player);
        if(inRange) {
            camManager.switchCamera(newCam);
        } else{
            camManager.resetCamera(newCam);
        }
    }
}
