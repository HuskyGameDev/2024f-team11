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
    private EventInstance doorOpen;
    private EventInstance doorClose;
    float timeCount = 0.0f;
    bool playerInteraction = false;

    private void Start()
    {
        startRotation = Quaternion.Euler(0f, (transform.rotation.y * Mathf.PI * Mathf.Rad2Deg), 0f);
        transform.rotation = Quaternion.Euler(0f, (transform.rotation.y * Mathf.PI * Mathf.Rad2Deg) + -90f, 0f);
        doorOpen = AudioManager.Instance.CreateInstance(FMODEvents.Instance.doorOpen);
        doorClose = AudioManager.Instance.CreateInstance(FMODEvents.Instance.doorClose);
    }

    //Open or close the door in the correct direction
    public void Toggle()
    {
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
        if (isOpen && playerInteraction)
        {
            transform.localRotation = Quaternion.Lerp(this.transform.localRotation, Quaternion.Euler(0f, (startRotation.y * Mathf.PI * Mathf.Rad2Deg), 0f), timeCount * 0.1f);
            timeCount += Time.deltaTime;
            if (timeCount >= 1.0f || transform.localRotation == Quaternion.Euler(0f, (startRotation.y * Mathf.PI * Mathf.Rad2Deg), 0f))
            {
                isOpen = false;
                playerInteraction = false;
                timeCount = 0;
            }
        }
        else if (!isOpen && playerInteraction)
        {
            transform.localRotation = Quaternion.Lerp(this.transform.localRotation, Quaternion.Euler(0f, -90f + (startRotation.y * Mathf.PI * Mathf.Rad2Deg), 0f), timeCount * 0.1f);
            timeCount += Time.deltaTime;
            if (timeCount >= 1.0f || transform.localRotation == Quaternion.Euler(0f, -90f + (startRotation.y * Mathf.PI * Mathf.Rad2Deg), 0f))
            {
                isOpen = true;
                playerInteraction = false;
                timeCount = 0;
            }
        }
    }
}
