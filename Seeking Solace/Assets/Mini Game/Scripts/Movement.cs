using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    public CharacterController characterController;
    public Transform jumpCheck;
    public Transform cam;
    public LayerMask groundMask;

    const float regularSpeed = 15f;

    public float speed;
    float gravity = -15f;
    public float jumpHeight = 3f;
    public float groundDistance = 0.4f;
    public float turnTime = 0.5f;
    bool isGrounded;
    public bool hasGlided;
    public bool isGliding;
    bool waiting = false;
    float turnVelocity;
    float timeWaited = 0f;
    Vector3 velocity;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        speed = regularSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        if(isGliding){
            vertical = 1f; //Mathf.Clamp(vertical, 0f, 1f);
            horizontal = 0f; //Mathf.Clamp(horizontal, -0.5f, 0.5f);
        }

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        
        if (direction.magnitude >= 0.1f) { 
            float targetAngle = Mathf.Atan2(direction.x,direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocity, turnTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            characterController.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        isGrounded = Physics.CheckSphere(jumpCheck.position, groundDistance, groundMask);

        if(Input.GetButtonDown("Jump") && isGrounded){
                velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        if(Input.GetButtonDown("Jump") && !hasGlided && velocity.y < 0){
            hasGlided = true;
            isGliding = true;
            gravity = 0;
            velocity.y = -7.5f;
            waiting = true;
            StartCoroutine(Glide());
        }

        if(isGliding){
            if(timeWaited >= 1){
                if(Input.GetButtonDown("Jump") || isGrounded){
                    isGliding = false;
                    gravity = -15;
                    speed = regularSpeed;  
                    waiting = false;
                    timeWaited = 0f;
                } 
            }
        }

        if(waiting){
            timeWaited += 0.1f;
        }

        if(!isGliding){
            velocity.y += gravity * Time.deltaTime;
        }

        if(isGrounded && velocity.y < 0){
            hasGlided = false;
            velocity.y = -2;
        }

        characterController.Move(velocity * Time.deltaTime);

    }

    IEnumerator Glide()
    {
        yield return new WaitForSeconds(0.3f);
        while(speed < regularSpeed * 2){
            speed += 1f;
            if(!waiting){
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;

        /*
        gravity = 0;
        velocity.y = -5f;
        yield return new WaitForSeconds(0.5f);
        if(Input.GetButtonDown("Jump") || isGrounded){
            Debug.Log("Not Gliding");
            isGliding = false;
            gravity = -15;
            yield return null;
        }
        */
    }
    
}
