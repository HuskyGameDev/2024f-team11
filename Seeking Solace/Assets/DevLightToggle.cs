using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevLightToggle : MonoBehaviour
{
    public float intensity;
    // Start is called before the first frame update
    void Start()
    {
        RenderSettings.ambientIntensity = 1;
        RenderSettings.fog = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L)) {
            RenderSettings.fog = !RenderSettings.fog;   //Toggle fog
            if(RenderSettings.ambientIntensity != 1) {  //Toggle brightness
                RenderSettings.ambientIntensity = 1;
            }
            else{
                RenderSettings.ambientIntensity = intensity;
            }
        }
    }
}
