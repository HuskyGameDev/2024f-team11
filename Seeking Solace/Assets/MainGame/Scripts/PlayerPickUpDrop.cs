using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//THIS SCRIPT IS NOT BEING USED FOR PICKUP, USE "PICKUP DROP" INSTEAD
public class PlayerPickupDrop : MonoBehaviour
{
    public Transform player;           // Reference to the player or camera transform
    public float pickupRange = 3f;     // How close the player needs to be to pick up the item
    public Transform itemHoldPosition; // The position where the item will be held
    private GameObject pickedUpItem;   // The currently picked up item
    private Rigidbody itemRb;          // Rigidbody of the item
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Press 'E' to interact (pick up/drop)
        {
            if (pickedUpItem == null)
            {
                TryPickUpItem();
            }
            else
            {
                DropItem();
            }
        }
    }

    void TryPickUpItem()
    {
        RaycastHit hit;
        // Cast a ray from the player's position to detect objects in front of them
        if (Physics.Raycast(player.position, player.forward, out hit, pickupRange))
        {
            // Check if the object is "pickable" by having a tag or specific layer
            if (hit.collider.CompareTag("Pickable"))
            {
                pickedUpItem = hit.collider.gameObject;
                itemRb = pickedUpItem.GetComponent<Rigidbody>();

                if (itemRb != null)
                {
                    itemRb.isKinematic = true; // Disable physics while holding the object
                    pickedUpItem.transform.position = itemHoldPosition.position;
                    pickedUpItem.transform.SetParent(itemHoldPosition);
                }
            }
        }
    }

    void DropItem()
    {
        if (pickedUpItem != null)
        {
            pickedUpItem.transform.SetParent(null); // Unparent the item
            itemRb.isKinematic = false; // Re-enable physics
            itemRb = null;
            pickedUpItem = null;
        }
    }
}



