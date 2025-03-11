using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class Drawer : MonoBehaviour
{
    private bool isOpen = false;
    private bool isLocked = false;
    private KeyColor lockColor = KeyColor.red;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private EventInstance drawerOpen;
    private EventInstance drawerClose;
    float timeCount = 0.0f;
    float initialTime = 0.0f;
    bool playerInteraction = false;
    bool isMoving = false;
    public float distanceX = 0f;
    public float distanceZ = 0f;

    private void OnEnable()
    {
        InteractWithObject.OnObjectInteraction += Toggle;
    }

    private void OnDisable()
    {
        InteractWithObject.OnObjectInteraction -= Toggle;
    }

    private void Start()
    {
        startPosition = transform.localPosition;
        targetPosition = startPosition + new Vector3(distanceX, 0f, distanceZ);
        drawerOpen = AudioManager.Instance.CreateInstance(FMODEvents.Instance.drawerOpen);
        drawerClose = AudioManager.Instance.CreateInstance(FMODEvents.Instance.drawerClose);
    }

    //Open or close the door in the correct direction
    public void Toggle(GameObject drawerObj)
    {
        if (gameObject != drawerObj) return;
        //Get player and pickup script
        GameObject playerObject = GameObject.Find("Player");
        //PickupDrop pickScript = playerObject.GetComponent<PickupDrop>();
        //GameObject heldItem = pickScript.getPickedItem();

        if (isLocked)
        {
            //if (heldItem.GetComponent<Key>() != null) //If held item is a key (has key component)
            //{
            //    KeyColor keyColor = heldItem.GetComponent<Key>().GetColor();
            //    if (keyColor == lockColor) //If keycolor matches door lock color
            //    {
            //        isLocked = !isLocked;
            //        pickScript.ConsumeHeldItem();
            //        isOpen = !isOpen;
            //        //Play unlock sound and maybe some effect for key disappearing
            //    }
            //}
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
            if (!isOpen && !AudioManager.Instance.IsPlaying(drawerClose))
            {
                drawerClose.start();
                drawerOpen.stop(STOP_MODE.ALLOWFADEOUT);
            }
            else if (isOpen && !AudioManager.Instance.IsPlaying(drawerOpen))
            {
                drawerOpen.start();
                drawerClose.stop(STOP_MODE.ALLOWFADEOUT);
            }
            timeCount += Time.deltaTime;
            initialTime += Time.deltaTime;
            timeCount = Mathf.Clamp01(timeCount); // Ensure value is between 0 and 1
            Vector3 newPosition = isOpen ? targetPosition : startPosition;
            transform.localPosition = Vector3.Lerp(transform.localPosition, newPosition, timeCount / 4.0f);

            // Check if the door has sufficiently rotated
            if (transform.localPosition == newPosition)
            {
                transform.localPosition = newPosition; // Snap to final position
                isMoving = false; // Stop movement
                initialTime = 0.0f;
            }
        }
    }
}
