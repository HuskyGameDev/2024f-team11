using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    public Image cursor;

    private void OnEnable()
    {
        ToggleDoor.OnHoverDoor += ChangeCursor;
    }

    private void OnDisable()
    {
        ToggleDoor.OnHoverDoor -= ChangeCursor;
    }

    void ChangeCursor()
    {

    }
}
