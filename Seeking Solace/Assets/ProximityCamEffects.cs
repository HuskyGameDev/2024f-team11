using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PSXShaderKit;

public class ProximityCamEffects : MonoBehaviour
{
    public PSXPostProcessEffect effect;
    public Camera camera;

    public float startingFov;

    public float maxFov;

    public float zoomProximity;
    public Transform monsterTransform;
    // Start is called before the first frame update
    void Start()
    {
        camera.fieldOfView = startingFov;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, monsterTransform.position) < zoomProximity) {
            camera.fieldOfView = Mathf.Clamp(camera.fieldOfView * (1 + (maxFov - camera.fieldOfView) * 0.0005f), startingFov, maxFov);
            effect._PixelationFactor = Mathf.Clamp(effect._PixelationFactor - 0.001f, 0.2f, 0.3f);
        }
        else {
            camera.fieldOfView = Mathf.Clamp(camera.fieldOfView * (1 - (camera.fieldOfView - startingFov) * 0.0005f), startingFov, maxFov);
            effect._PixelationFactor = Mathf.Clamp(effect._PixelationFactor + 0.001f, 0.2f, 0.3f); ;
        }
    }

}
