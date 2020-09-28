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
    public GameObject RespawnPoint;

    public float speed = 6f;
    public float sprintSpeed = 9f;
    float moveSpeed;
    bool _isSprinting = false;
    float moveDirection = 0f;

    public float grav = 9.8f;
    public float jumpSpeed = 12f;
    private float velocityY = 0f;
    bool _isJumping = false;
    [SerializeField] AudioClip _landingSound;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    bool _isMoving = false;

    public bool _isAlive = true;

    [SerializeField] PushBar PushBar;
    [SerializeField] int maxPushStrength = 3;
    public int currentPushStrength;

    private void Start()
    {
        Idle?.Invoke();
        _isAlive = true;
        currentPushStrength = 1;
        PushBar.SetMaxStrength(maxPushStrength);
        PushBar.SetStrength(currentPushStrength);
    }

    // Update is called once per frame
    void Update()
    {
        if (_isAlive)
        {
            //input to change push strength
            if (Input.GetKeyDown(KeyCode.E))
            {
                currentPushStrength += 1;
                PushBar.SetStrength(currentPushStrength);
                Debug.Log("Increased Push strength to " + currentPushStrength);
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                currentPushStrength -= 1;
                PushBar.SetStrength(currentPushStrength);
                Debug.Log("Decreased Push strength to " + currentPushStrength);
            }
            //keep push strength within restraints
            if (currentPushStrength > 3)
            {
                currentPushStrength = 3;
            }
            if (currentPushStrength < 1)
            {
                currentPushStrength = 1;
            }

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
            if (_isSprinting)
            {
                moveSpeed = sprintSpeed;
            }
            else
            {
                moveSpeed = speed;
            }

            if (controller.isGrounded)
            {
                velocityY = 0;
                //alow player to jump while grounded
                if (Input.GetButtonDown("Jump") && velocityY < 0.1f)
                {
                    velocityY = jumpSpeed;
                    StartJump?.Invoke();
                }
            }
            else if (!controller.isGrounded)
            {
                //apply gravity on player if they are not on the ground
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

                //move player in a direction if they are alive
                if (_isAlive)
                {
                    Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                    controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
                }
            }
            else
            {
                if (!_isJumping)
                {
                    CheckIfStoppedMoving();
                }
            }

            if (velocityY < -5f)
            {
                Falling?.Invoke();
            }

            //move in y direction if the player is alive
            if (_isAlive)
            {
                direction.y = velocityY;
                controller.Move(direction * Time.deltaTime);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        AudioHelper.PlayClip2D(_landingSound, 0.15f);
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
