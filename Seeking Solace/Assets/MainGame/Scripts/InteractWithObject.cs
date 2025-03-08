using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractWithObject : MonoBehaviour
{
    public static event Action<GameObject> OnObjectInteraction;
    public static event Action OnHoverInteractable;
    public static event Action OnOutsideInteractable;
    Transform mainCamera;
    int LayerName;
    bool isPaused = false;

    private void OnEnable()
    {
        GUIManager.OnPause += PauseFeedback;
        GUIManager.OnUnpause += ResumeFeedback;
    }

    private void OnDisable()
    {
        GUIManager.OnPause -= PauseFeedback;
        GUIManager.OnUnpause -= ResumeFeedback;
    }

    private void Awake()
    {
        LayerName = LayerMask.NameToLayer("Interactable");
        mainCamera = Camera.main.transform;
    }

    void Update()
    {
        if (!isPaused)
        {
            RaycastHit hit;

            // Cast a ray from the player's position to detect objects in front of them
            if (Physics.Raycast(mainCamera.position, mainCamera.forward, out hit, 3f))
            {
                if (hit.collider.gameObject.layer == LayerName)
                {
                    OnHoverInteractable?.Invoke();
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        GameObject objHit;
                        if (!hit.collider.transform.GetComponent<Wardrobe>())
                            objHit = hit.collider.transform.parent.gameObject;
                        else
                            objHit = hit.collider.gameObject;
                        if (hit.collider.transform.GetComponent<Door>())
                            objHit = hit.collider.gameObject;
                        OnObjectInteraction?.Invoke(objHit);
                    }
                }
                else
                {
                    OnOutsideInteractable?.Invoke();
                }
            }
            else
            {
                OnOutsideInteractable?.Invoke();
            }
        }
    }

    void PauseFeedback()
    {
        isPaused = true;
    }

    void ResumeFeedback()
    {
        isPaused = false;
    }
}
