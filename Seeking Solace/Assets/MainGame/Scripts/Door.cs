using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class Door : MonoBehaviour
{
    private bool isOpen = false;
    private bool isLocked = false;
    private KeyColor lockColor = KeyColor.red;
    public Quaternion startRotation;
    public Quaternion targetRotation;
    public bool rotateClockwise = false;
    private EventInstance doorOpen;
    private EventInstance doorClose;
    float doorOpenTime = 5.0f;
    float timeCount = 0.0f;
    float initialTime = 0.0f;
    bool playerInteraction = false;
    bool isMoving = false;
    bool playerIsColliding = false;

    private void OnEnable()
    {
        InteractWithObject.OnObjectInteraction += Toggle;
    }

    private void Start()
    {
        transform.localRotation = startRotation;
        targetRotation = rotateClockwise ? Quaternion.Euler(0f, 90f + startRotation.eulerAngles.y, 0f) : Quaternion.Euler(0f, -90f + startRotation.eulerAngles.y, 0f);
        doorOpen = AudioManager.Instance.CreateInstance(FMODEvents.Instance.doorOpen);
        doorClose = AudioManager.Instance.CreateInstance(FMODEvents.Instance.doorClose);
    }

    //Open or close the door in the correct direction
    public void Toggle(GameObject doorObj)
    {
        if (gameObject != doorObj) return;
        //Get player and pickup script
        GameObject playerObject = GameObject.Find("Player");
        PickupDrop pickScript = playerObject.GetComponent<PickupDrop>();
        GameObject heldItem = pickScript.getPickedItem();

        if (isLocked)
        {
            if (heldItem.GetComponent<Key>() != null) //If held item is a key (has key component)
            {
                KeyColor keyColor = heldItem.GetComponent<Key>().GetColor();
                if(keyColor == lockColor) //If keycolor matches door lock color
                {
                    isLocked = !isLocked;
                    pickScript.ConsumeHeldItem();
                    isOpen = !isOpen;
                    //Play unlock sound and maybe some effect for key disappearing
                }
            }
        }
        else
        {
            playerInteraction = true;
        }
    }

    private void Update()
    {
        if (playerInteraction)
        {
            playerInteraction = false; // Consume input immediately
            isOpen = !isOpen; // Toggle door state
            isMoving = true; // Start moving
            timeCount = 0.0f; // Reset interpolation
        }

        if (isMoving)
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
            if (!isOpen && !AudioManager.Instance.IsPlaying(doorClose))
            {
                doorClose.start();
                doorOpen.stop(STOP_MODE.ALLOWFADEOUT);
            }
            else if (isOpen && !AudioManager.Instance.IsPlaying(doorOpen))
            {
                doorOpen.start();
                doorClose.stop(STOP_MODE.ALLOWFADEOUT);
            }
            timeCount += Time.deltaTime;
            initialTime += Time.deltaTime;
            timeCount = Mathf.Clamp01(timeCount); // Ensure value is between 0 and 1
            Quaternion newRotation = isOpen ? targetRotation : startRotation;
            transform.localRotation = Quaternion.Slerp(transform.localRotation, newRotation, timeCount / 4.0f);

            // Check if the door has sufficiently rotated
            if (transform.localRotation == newRotation)
            {
                transform.localRotation = newRotation; // Snap to final position
                isMoving = false; // Stop movement
                initialTime = 0.0f;
            }
        }
        else
        {
            gameObject.GetComponent<BoxCollider>().enabled = true;
        }
    }
}
