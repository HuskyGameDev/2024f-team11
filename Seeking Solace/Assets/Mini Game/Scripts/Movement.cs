using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    public CharacterController characterController;
    public Transform jumpCheck;
    public Transform cam;
    public LayerMask groundMask;

    public float speed = 6f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float groundDistance = 0.4f;
    public float turnTime = 0.5f;
    bool isGrounded;
    float turnVelocity;
    Vector3 velocity;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f) { 
            float targetAngle = Mathf.Atan2(direction.x,direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocity, turnTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            characterController.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        isGrounded = Physics.CheckSphere(jumpCheck.position, groundDistance, groundMask);
        velocity.y += gravity * Time.deltaTime;

        if(Input.GetButtonDown("Jump") && isGrounded){
                velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        if(isGrounded && velocity.y < 0){
            velocity.y = -2;
        }

        characterController.Move(velocity * Time.deltaTime);

    }
}
