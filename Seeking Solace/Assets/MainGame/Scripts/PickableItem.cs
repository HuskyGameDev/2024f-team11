using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : MonoBehaviour
{
    private Rigidbody rb;

    // Initialize method to replace Start()
    public void Initialize()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Method called when the object is picked up
    public void OnPickUp()
    {
        rb.isKinematic = true;  // Disable physics when picked up
        Debug.Log(gameObject.name + " picked up!");
        // Additional behavior, like changing color or playing a sound
    }

    // Method called when the object is dropped
    public void OnDrop()
    {
        rb.isKinematic = false;  // Re-enable physics when dropped
        Debug.Log(gameObject.name + " dropped!");
        // Additional behavior, like reverting color or playing a sound
    }
}





