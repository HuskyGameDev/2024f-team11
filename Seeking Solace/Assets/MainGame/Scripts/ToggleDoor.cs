using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleDoor : MonoBehaviour
{
    public Transform camera; //Player camera reference
    public float interactRange = 3f; //How far away you can open doors from

    public static event Action OnHoverDoor;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) //Press 'E' to toggle doors}
        {
            Debug.Log("Trying to open door");
            TryOpenDoor();
        }
    }

    void TryOpenDoor()
    {
        Debug.Log("Raycasting");
        RaycastHit hit;

        if(Physics.Raycast(camera.position, camera.forward, out hit, interactRange))
        {
            if(hit.collider.CompareTag("Door"))
            {
                OnHoverDoor?.Invoke();
                hit.collider.GetComponent<Door>().Toggle(hit.collider.gameObject);
            }
        }
    }
}
