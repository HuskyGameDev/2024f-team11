using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hide : MonoBehaviour
{
    public Transform camera;

    public static event Action<GameObject, GameObject> OnHide;
    public static event Action<GameObject, GameObject> OnUnhide;

    public static Hide instance;

    private Transform hideSpot; // Current hide location
    private bool isHiding = false;
    private GameObject player;
    private Camera playerCamera;
    private Transform cameraThirdPersonPoint; // Position for third-person camera
    private Vector3 previousPosition;

    GameObject objHit;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerCamera = Camera.main;
    }

    private void Update()
    {
        if (isHiding && Input.GetKeyDown(KeyCode.E)) // Exit hiding with "E"
        {
            ExitHiding();
        }
        else if (!isHiding && Input.GetKeyDown(KeyCode.E))
        {
            TryToHide();
        }
    }

    public void ExitHiding()
    {
        OnUnhide?.Invoke(this.gameObject, objHit);

        isHiding = false;

        transform.position = previousPosition;

        Debug.Log("Player left hiding spot!");
    }

    void TryToHide()
    {
        RaycastHit hit;

        // Cast a ray from the player's position to detect objects in front of them
        if (Physics.Raycast(camera.position, camera.forward, out hit, 3f))
        {
            if (hit.collider.CompareTag("Hidable"))
            {
                // Hide
                if (!hit.collider.gameObject.GetComponent<Wardrobe>())
                    objHit = hit.collider.transform.parent.gameObject;
                else
                    objHit = hit.collider.gameObject;

                isHiding = true;
                OnHide?.Invoke(this.gameObject, objHit);
                previousPosition = gameObject.transform.position;
            }
        }
    }
}
