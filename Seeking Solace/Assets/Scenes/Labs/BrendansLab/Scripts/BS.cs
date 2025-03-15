using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMOD.Studio;

public class BS : MonoBehaviour
{         
    public float pickupRange = 3f;     // How close the player needs to be to pick up the item
    public Transform itemHoldPosition; // The position where the item will be held
    private GameObject pickedUpItem;   // The currently picked up item
    private Rigidbody itemRb;          // Rigidbody of the item
    private EventInstance pickUpBS;
    float timeCount = 0.0f;

    private void OnEnable()
    {
        InteractWithObject.OnObjectInteraction += Interact;
    }

    private void OnDisable()
    {
        InteractWithObject.OnObjectInteraction -= Interact;
    }

    private void Start()
    {
        pickUpBS = AudioManager.Instance.CreateInstance(FMODEvents.Instance.pickUpBS);
    }

    void Update()
    {
        if (pickedUpItem)
        {
            itemHoldPosition.transform.position = Camera.main.transform.position + (Camera.main.transform.forward * 1.20f);
            itemHoldPosition.transform.rotation = Camera.main.transform.rotation;

            pickedUpItem.transform.position = Vector3.Lerp(pickedUpItem.transform.position, itemHoldPosition.position, timeCount);
            pickedUpItem.transform.localRotation = Quaternion.Lerp(pickedUpItem.transform.localRotation, Quaternion.Euler(60f, 180f, 0f), timeCount);
            timeCount += Time.deltaTime;

            // BS Logic Goes Here

            if (Input.GetKeyDown(KeyCode.R))
                SceneManager.LoadScene(2);

            // End BS Logic
        }
        else
        {
            timeCount = 0.0f;
        }
    }

    void Interact(GameObject obj)
    {
        if (gameObject != obj) return;

        // Play Interaction Animations/Sounds Here
        if (pickedUpItem == null)
        {
            pickedUpItem = obj;
            itemRb = pickedUpItem.GetComponent<Rigidbody>();

            if (itemRb != null)
            {
                itemRb.isKinematic = true; // Disable physics while holding the object
                pickedUpItem.transform.SetParent(itemHoldPosition);
            }

            gameObject.GetComponent<Animator>().SetTrigger("Open");
            pickUpBS.start();
        }
        else
        {
            DropItem();
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
            gameObject.GetComponent<Animator>().SetTrigger("Close");
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
