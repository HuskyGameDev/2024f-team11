using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleDoor : MonoBehaviour
{
    public Transform camera; //Player camera reference
    public float interactRange = 6f; //How far away you can open doors from

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) //Press 'F' to toggle doors}
        {
            TryOpenDoor();
        }
    }

    void TryOpenDoor()
    {
        RaycastHit hit;

        if(Physics.Raycast(camera.position, camera.forward, out hit, interactRange))
        {
            if(hit.collider.CompareTag("Door"))
            {
                hit.collider.GetComponent<Door>().Toggle(this.transform);
            }
        }
    }
}
