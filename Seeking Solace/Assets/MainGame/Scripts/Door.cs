using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private bool isOpen = false;
    private bool isLocked = true;
    private KeyColor lockColor = KeyColor.red;

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
            transform.rotation = transform.parent.rotation;
            isOpen = false;
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
                    //Play locked sound
                }
            }
            else
            {
                OpenDoor(playerRelativePos);
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
