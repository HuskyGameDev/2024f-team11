using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamSwitch : MonoBehaviour
{
    public CinemachineFreeLook startCam;
    private CinemachineVirtualCamera tempCam;

    // Start is called before the first frame update
    void Start()
    {
        startCam.Priority = 20;
    }

   public void switchCamera(CinemachineVirtualCamera cam){
    startCam.Priority = 10;
    tempCam = cam;
    tempCam.Priority = 20;
   }

   public void resetCamera(){
    tempCam.Priority = 10;
    startCam.Priority = 20;
   }
}
