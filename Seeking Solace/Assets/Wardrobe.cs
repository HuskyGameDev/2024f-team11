using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wardrobe : MonoBehaviour
{
    GameObject playerCheck;
    GameObject currentInteractedObject;
    Transform leftDoorTransform;
    Quaternion leftStartRotation;
    Quaternion leftTargetRotation;
    Transform rightDoorTransform;
    Quaternion rightStartRotation;
    Quaternion rightTargetRotation;
    MeshRenderer renderer;
    MeshRenderer rendererTwo;
    Color newColor;
    float opacity = 0.5f;
    bool isOpen = false;
    bool isMoving = false;
    public bool doorDirectionToggle = false;
    bool interaction = false;
    float timeCount = 0.0f;

    private void OnEnable()
    {
        InteractWithObject.OnObjectInteraction += Interaction;
    }

    private void OnDisable()
    {
        InteractWithObject.OnObjectInteraction -= Interaction;
    }

    void Start()
    {
        leftDoorTransform = gameObject.transform.GetChild(0);
        rightDoorTransform = gameObject.transform.GetChild(1);
        renderer = leftDoorTransform.gameObject.GetComponent<MeshRenderer>();
        newColor = renderer.material.color;
        rendererTwo = rightDoorTransform.gameObject.GetComponent<MeshRenderer>();
        newColor.a = 1f;
        rendererTwo.material.color = newColor;
        leftStartRotation = Quaternion.Euler(0f, 0f, 0f);
        leftTargetRotation = doorDirectionToggle ? Quaternion.Euler(0f, -90f + leftStartRotation.eulerAngles.y, 0f) : Quaternion.Euler(0f, 90f + leftStartRotation.eulerAngles.y, 0f);
        rightStartRotation = Quaternion.Euler(0f, 0f, 0f);
        rightTargetRotation = doorDirectionToggle ? Quaternion.Euler(0f, 90f + rightStartRotation.eulerAngles.y, 0f) : Quaternion.Euler(0f, -90f + rightStartRotation.eulerAngles.y, 0f);
    }

    private void Update()
    {
        if (interaction)
        {
            interaction = false; // Consume input immediately
            isOpen = !isOpen; // Toggle door state
            isMoving = true; // Start moving
            timeCount = 0.0f; // Reset interpolation
        }

        if (isMoving)
        {
            timeCount += Time.deltaTime;
            timeCount = Mathf.Clamp01(timeCount); // Ensure value is between 0 and 1
            Quaternion newLeftRotation = isOpen ? leftTargetRotation : leftStartRotation;
            Quaternion newRightRotation = isOpen ? rightTargetRotation : rightStartRotation;
            leftDoorTransform.localRotation = Quaternion.Slerp(leftDoorTransform.localRotation, newLeftRotation, timeCount / 4.5f);
            rightDoorTransform.localRotation = Quaternion.Slerp(rightDoorTransform.localRotation, newRightRotation, timeCount / 4.5f);

            // Check if the door has sufficiently rotated
            if (leftDoorTransform.localRotation == newLeftRotation && rightDoorTransform.localRotation == newRightRotation)
            {
                isMoving = false; // Stop movement
            }
        }

        if (!isOpen && Vector3.Distance(Camera.main.transform.position, this.gameObject.transform.position) < 2f)
        {
            newColor.a = 0.5f;
            renderer.material.color = newColor;
            rendererTwo.material.color = newColor;
        }
        else
        {
            newColor.a = 1.0f;
            renderer.material.color = newColor;
            rendererTwo.material.color = newColor;
        }
    }

    public void Interaction(GameObject triggeringWardrobe)
    {
        if (this.gameObject != triggeringWardrobe) return;

        interaction = true;
        currentInteractedObject = triggeringWardrobe;
        leftDoorTransform = currentInteractedObject.transform.GetChild(0);
        rightDoorTransform = currentInteractedObject.transform.GetChild(1);
    }
}
