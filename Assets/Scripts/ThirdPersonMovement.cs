using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ThirdPersonMovement : MonoBehaviour
{
    public event Action Idle = delegate { };
    public event Action StartRunning = delegate { };
    public event Action Sprinting = delegate { };
    public event Action StartJump = delegate { };
    public event Action Falling = delegate { };
    public event Action Landing = delegate { };

    public GroundCheck GroundCheck;
    public CharacterController controller;
    public Transform cam;

    public float speed = 6f;
    public float sprintSpeed = 9f;
    float moveSpeed;
    bool _isSprinting = false;
    float moveDirection = 0f;

    public float grav = 9.8f;
    public float jumpSpeed = 12f;
    private float velocityY = 0f;
    bool _isJumping = false;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    bool _isMoving = false;

    private void Start()
    {
        Idle?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        //get player input for movement
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        //create direction of movement based on player input
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        moveDirection = direction.magnitude;

        if (controller.isGrounded)
        {
            _isJumping = false;
        }

        //check to see if player is holding Left Shift to sprint, if player releases go back to normal run
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _isSprinting = true;
        }
        else
        {
            _isSprinting = false;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && direction.magnitude >= 0.1f)
        {
            CheckIfSprinting();
        } 
        else if (Input.GetKeyUp(KeyCode.LeftShift) && direction.magnitude >= 0.1f)
        {
            StartRunning?.Invoke();
        }
        if (!_isSprinting && direction.magnitude >= 0.1)
        {
            CheckIfStartedMoving();
        }

        //if sprinting change the movement speed
        if(_isSprinting)
        {
            moveSpeed = sprintSpeed;
        }
        else
        {
            moveSpeed = speed;
        }

        //apply gravity on player if they are not on the ground
        if (controller.isGrounded)
        {
            velocityY = 0;
            if (Input.GetButtonDown("Jump"))
            {
                velocityY = jumpSpeed;
                StartJump?.Invoke();
            }
        }
        else if (!controller.isGrounded)
        {
            velocityY -= grav * Time.deltaTime;
            _isJumping = true;
        }

        if (direction.magnitude >= 0.1f)
        {
            if (!_isJumping)
            {
                CheckIfStartedMoving();
            }

 
            //rotate player to face direction that they are moving in
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            //smooth rotation
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //move player in a direction
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
        }
        else
        {
            if (!_isJumping)
            {
                CheckIfStoppedMoving();
            }
        }

        if(velocityY < -5f)
        {
            Falling?.Invoke();
        }

        direction.y = velocityY;
        controller.Move(direction * Time.deltaTime);
    }

    //when player collides with the ground run proper animation
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            Landing?.Invoke();
        }
    }

    private void CheckIfStartedMoving()
    {
        if(_isMoving == false) 
        {
            // our velocity says we're moving but we previously were not
            //this means we've started moving!
            StartRunning?.Invoke();
            Debug.Log("Started");
        }
        _isMoving = true;
    }

    private void CheckIfStoppedMoving()
    {
        if(_isMoving == true)
        {
            //our velocity says we're moving, but previously were
            //this means we've stoppred!
            Idle?.Invoke();
            Debug.Log("Stopped");
        }
        _isMoving = false;
    }

    private void CheckIfSprinting()
    {
        if(_isSprinting == true)
        {
            Sprinting?.Invoke();
            Debug.Log("Sprinting");
        }
    }
}
