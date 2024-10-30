using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerController : MonoBehaviour
{
    // Variables to define open and close positions
    [SerializeField] private Transform openPosition;
    [SerializeField] private Transform closePosition;
    [SerializeField] private float speed = 5f;

    // Variable to track drawer state
    private bool isOpen = false;
    private bool isMoving = false;

    // Distance threshold to consider as "reached"
    private const float stopDistance = 0.01f;

    // Update is called once per frame
    void Update()
    {
        // If the drawer is moving, smoothly move it to the target position
        if (isMoving)
        {
            MoveDrawer();
        }
    }

    // Function to toggle the drawer state
    public void ToggleDrawer()
    {
        isOpen = !isOpen;
        isMoving = true;
    }

    // Move drawer towards the target position
    private void MoveDrawer()
    {
        // Set target position based on drawer state
        Transform targetPosition = isOpen ? openPosition : closePosition;

        // Move the drawer towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, speed * Time.deltaTime);

        // Stop moving when the drawer is close enough to the target position
        if (Vector3.Distance(transform.position, targetPosition.position) <= stopDistance)
        {
            isMoving = false;  // Stop the movement
            transform.position = targetPosition.position;  // Snap exactly to the target position
        }
    }

    // Detect player interaction to trigger drawer
    private void OnMouseDown()
    {
        if (!isMoving)  // Prevents triggering while the drawer is moving
        {
            ToggleDrawer();
        }
    }
}


