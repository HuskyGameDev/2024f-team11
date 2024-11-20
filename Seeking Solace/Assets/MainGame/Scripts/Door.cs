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

    private void Start()
    {
        startRotation = transform.rotation; //Set closed position to initial position (doors start closed)
        doorOpen = AudioManager.Instance.CreateInstance(FMODEvents.Instance.doorOpen);
        doorClose = AudioManager.Instance.CreateInstance(FMODEvents.Instance.doorClose);
    }

    //Open or close the door in the correct direction (away from player)
    public void Toggle(Transform player)
    {
        //Player's position relative to the door (used to get correct opening direction)
        Vector3 playerRelativePos = this.transform.InverseTransformPoint(player.position);

        //Get player and pickup script
        GameObject playerObject = GameObject.Find("Player");
        PickupDrop pickScript = playerObject.GetComponent<PickupDrop>();
        GameObject heldItem = pickScript.getPickedItem();

        if (isOpen)
        {
            //Close the door (reset rotation to match doorframe)
            transform.rotation = startRotation;
            isOpen = false;
            doorClose.start();
        }
        else
        {
            if (isLocked)
            {
                if (heldItem.GetComponent<Key>() != null) //If held item is a key (has key component)
                {
                    KeyColor keyColor = heldItem.GetComponent<Key>().GetColor();
                    if(keyColor == lockColor) //If keycolor matches door lock color
                    {
                        isLocked = !isLocked;
                        pickScript.ConsumeHeldItem();
                        OpenDoor(playerRelativePos);
                        //Play unlock sound and maybe some effect for key disappearing
                    }
                }
                else
                {
                    
                }
            }
            else
            {
                OpenDoor(playerRelativePos);
                doorOpen.start();
            }
        }

    }

    private void OpenDoor(Vector3 playerRelativePos)
    {
        if (playerRelativePos.z <= 0)
        {
            //Open clockwise
            transform.Rotate(0, 90, 0, Space.Self);
        }
        else
        {
            //Open counterclockwise
            transform.Rotate(0, -90, 0, Space.Self);
        }
        isOpen = true;
    }
}
