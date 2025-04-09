using System;
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
    bool playable = false;
    bool isInRoom = false;

    public static event Action<bool> BSPickedUp;
    public static event Action<bool> BSDropped;

    private void OnEnable()
    {
        InteractWithObject.OnObjectInteraction += Interact;
        InteractWithObject.EnteredRoom += SetRoomState;
        InteractWithObject.ExitedRoom += SetRoomState;
    }

    private void OnDisable()
    {
        InteractWithObject.OnObjectInteraction -= Interact;
        InteractWithObject.EnteredRoom -= SetRoomState;
        InteractWithObject.ExitedRoom -= SetRoomState;
    }

    private void SetRoomState(bool state)
    {
        isInRoom = state;
    }

    private void Start()
    {
        pickUpBS = AudioManager.Instance.CreateInstance(FMODEvents.Instance.pickUpBS);
    }

    void Update()
    {
        if (GameManager.Instance.Objective == Objective.PLAY_BS)
        {
            playable = true;
        }
        else
        {
            playable = false;
        }

        if (pickedUpItem)
        {
            itemHoldPosition.transform.position = Camera.main.transform.position + (Camera.main.transform.forward * 1.20f) + (Camera.main.transform.right * 0.75f) + (-Camera.main.transform.up * 0.25f);
            itemHoldPosition.transform.rotation = Camera.main.transform.rotation;

            pickedUpItem.transform.position = Vector3.Lerp(pickedUpItem.transform.position, itemHoldPosition.position, timeCount);
            pickedUpItem.transform.localRotation = Quaternion.Lerp(pickedUpItem.transform.localRotation, Quaternion.Euler(60f, 180f, 0f), timeCount);
            timeCount += Time.deltaTime;

            // BS Logic Goes Here

            if (!isInRoom)
                GameManager.Instance.UpdateObjective(Objective.GO_TO_ROOM);
            else
                GameManager.Instance.UpdateObjective(Objective.PLAY_BS);

            if (playable && Input.GetKeyDown(KeyCode.R))
                SceneManager.LoadScene(2);
            if (Input.GetKeyDown(KeyCode.Q))
                DropItem();

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

            pickUpBS.start();
            if (!isInRoom)
                GameManager.Instance.UpdateObjective(Objective.GO_TO_ROOM);
            else
                GameManager.Instance.UpdateObjective(Objective.PLAY_BS);
            BSPickedUp?.Invoke(true);
        }
    }

    void DropItem()
    {
        if (pickedUpItem != null)
        {
            pickedUpItem.transform.position = pickedUpItem.transform.parent.parent.position + Vector3.up * 2f;
            pickedUpItem.transform.SetParent(null); // Unparent the item
            itemRb.isKinematic = false; // Re-enable physics
            itemRb = null;
            pickedUpItem = null;
            //gameObject.GetComponent<Animator>().SetTrigger("Close");
            GameManager.Instance.UpdateObjective(Objective.FIND_BS);
            BSDropped?.Invoke(false);
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
