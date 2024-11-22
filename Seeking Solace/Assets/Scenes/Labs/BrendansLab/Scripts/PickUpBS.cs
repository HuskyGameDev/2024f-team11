using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PickUpBS : MonoBehaviour
{
    public Transform camera;           // Reference to the player or camera transform
    public float pickupRange = 3f;     // How close the player needs to be to pick up the item
    public Transform itemHoldPosition; // The position where the item will be held
    private GameObject pickedUpItem;   // The currently picked up item
    private Rigidbody itemRb;          // Rigidbody of the item
    float timeCount = 0.0f;
    [SerializeField] GameObject playText;

    void Update()
    {
        itemHoldPosition.transform.position = camera.position + (camera.forward * 0.80f);
        itemHoldPosition.transform.rotation = camera.rotation;

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

        if (pickedUpItem)
        {
            pickedUpItem.transform.position = Vector3.Lerp(pickedUpItem.transform.position, itemHoldPosition.position, timeCount);
            pickedUpItem.transform.localRotation = Quaternion.Lerp(pickedUpItem.transform.localRotation, Quaternion.Euler(60f, 180f, 0f), timeCount);
            timeCount += Time.deltaTime;

            //Key press ui
            playText.SetActive(true);
            if (Input.GetKeyDown(KeyCode.R))
                SceneManager.LoadScene("Mini game");
        }
        else
        {
            timeCount = 0.0f;
        }
    }

    void TryPickUpItem()
    {
        RaycastHit hit;

        // Cast a ray from the player's position to detect objects in front of them
        if (Physics.Raycast(camera.position, camera.forward, out hit, pickupRange))
        {
            Debug.Log(hit.collider);
            // Check if the object is "pickable" by having a tag or specific layer
            if (hit.collider.CompareTag("Pickable"))
            {
                AudioManager.Instance.PlaySound(hit.collider.gameObject, "BSPickUp");
                pickedUpItem = hit.collider.gameObject;
                itemRb = pickedUpItem.GetComponent<Rigidbody>();

                if (itemRb != null)
                {
                    itemRb.isKinematic = true; // Disable physics while holding the object
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

    public GameObject getPickedItem()
    {
        return pickedUpItem;
    }

    public void ConsumeHeldItem()
    {
        if (pickedUpItem != null)
        {
            Destroy(pickedUpItem);
            itemRb = null;
            pickedUpItem = null;
        }
    }
}
