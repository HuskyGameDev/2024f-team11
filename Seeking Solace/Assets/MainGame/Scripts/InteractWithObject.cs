using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractWithObject : MonoBehaviour
{
    public static event Action<GameObject> OnObjectInteraction;
    Transform camera;

    private void Awake()
    {
        camera = Camera.main.transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;

            // Cast a ray from the player's position to detect objects in front of them
            if (Physics.Raycast(camera.position, camera.forward, out hit, 3f))
            {
                GameObject objHit;

                if (!hit.collider.transform.GetComponent<Wardrobe>())
                    objHit = hit.collider.transform.parent.gameObject;
                else
                    objHit = hit.collider.gameObject;
                OnObjectInteraction?.Invoke(objHit);
            }
        }
    }
}
