using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2 : MonoBehaviour
{
    public CharacterController characterController;
    public Transform jumpCheck;
    public Transform headCheck;
    public LayerMask groundMask;
    public LayerMask ceilingMask;

    const float regularSpeed = 50f;
    public float speed;
    public float gravity = -15f;
    public float jumpHeight = 3f;
    public float groundDistance = 0.4f;
    public float ceilingDistance = 0.2f;
    public float fallMultiplier = 2.5f;
    bool isGrounded;
    bool hitCeiling;
    Vector3 velocity;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        speed = regularSpeed;
    }

    void Update()
    {
        // Get horizontal input
        float horizontal = Input.GetAxisRaw("Horizontal");
        Vector3 direction = new Vector3(-horizontal, 0f, 0f).normalized;

        // Movement (X-axis only)
        if (direction.magnitude >= 0.1f)
        {
            Vector3 moveDir = direction * speed;
            characterController.Move(moveDir * Time.deltaTime);

            // Rotate character to face movement direction
            if (horizontal < 0)
            {
                transform.rotation = Quaternion.Euler(0f, 90f, 0f); // Face right
            }
            else if (horizontal > 0)
            {
                transform.rotation = Quaternion.Euler(0f, 270f, 0f); // Face left
            }
        }

        // Ground check
        isGrounded = Physics.CheckSphere(jumpCheck.position, groundDistance, groundMask);

        // Ceiling check - only when moving upward
        hitCeiling = velocity.y > 0 && Physics.CheckSphere(headCheck.position, ceilingDistance, groundMask);

        // If we hit the ceiling, reverse vertical velocity to start falling
        if (hitCeiling)
        {
            velocity.y = -2f;
        }

        // Jump input
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        // Faster falling after peak
        if (velocity.y < 0) // When falling
        {
            velocity.y += gravity * fallMultiplier * Time.deltaTime;
        }
        else // When rising
        {
            velocity.y += gravity * Time.deltaTime;
        }

        // Reset vertical velocity when grounded
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Lock Z position
        Vector3 position = transform.position;
        position.z = 0;
        transform.position = position;

        // Apply movement
        characterController.Move(velocity * Time.deltaTime);
    }


}