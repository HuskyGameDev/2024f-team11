using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamSwitch : MonoBehaviour
{
    public CinemachineFreeLook startCam;

    // Start is called before the first frame update
    void Start()
    {
        startCam.Priority = 20;
    }

   public void switchCamera(CinemachineVirtualCamera cam){
    startCam.Priority = 10;
    cam.Priority = 20;
   }

   public void resetCamera(CinemachineVirtualCamera cam){
    cam.Priority = 10;
    startCam.Priority = 20;
   }
}
