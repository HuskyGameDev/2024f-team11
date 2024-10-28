using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private bool isOpen = false;

    //Open or close the door in the correct direction (away from player)
    public void Toggle(Transform player)
    {
        //Player's position relative to the door (used to get correct opening direction)
        Vector3 playerRelativePos = this.transform.InverseTransformPoint(player.position);

        if(isOpen)
        {
            //Close the door (reset rotation to match doorframe)
            transform.rotation = transform.parent.rotation;
            isOpen = false;
        }
        else
        {
            if(playerRelativePos.z <= 0)
            {
                //Open clockwise
                transform.Rotate(0, 90, 0, Space.Self);
            }
            else
            {
                //Open counterclockwise
                transform.Rotate(0,-90,0, Space.Self);
            }
            isOpen = true;
        }
    }
}
