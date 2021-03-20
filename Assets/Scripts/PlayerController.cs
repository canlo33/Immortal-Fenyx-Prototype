using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    //Caching Components
    private Animator anim;
    private Rigidbody rb;
    private PlayerInput playerInput;
    private new Transform camera;
    public ParticleSystem[] effects;

    //Dash Related Variables
    private float dashTimer;
    public float dashCooldown;
    private bool isDashing;
    private float dashStartTime;
    public float dashDuration;
    public Volume dashVolume;
    public float dashSpeed;

    //Movement Related Variables
    public float movementSpeed;
    private bool isMoving;
    public bool canMove = true;

    //Jump Related Variables
    private bool isJumping;
    public float jumpSpeed;
    private float defaultGravity;
    public float gravityMultiplier;

    //Rotation Related Variables
    public float rotationSpeed = 0.1f;
    private float smoothVelocity;

    //Melee Attack Related Variables
    public bool comboTimerOn;
    public int comboCount;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        camera = Camera.main.transform;
        defaultGravity = Physics.gravity.y;
    }
    // Update is called once per frame
    void Update()
    {
        AnimatorHandler();
        HandleDash();
        Jump();
        MeleeAttack();
    }
    private void FixedUpdate()
    {
        Movement();
    }
    void AnimatorHandler()
    {
        anim.SetFloat("horizontalMove", playerInput.HorizontalMove);
        anim.SetFloat("verticalMove", playerInput.VerticalMove);
        isMoving = !(playerInput.HorizontalMove == 0f && playerInput.VerticalMove == 0f);
        anim.SetBool("idle", !isMoving);
        anim.SetBool("run", isMoving);
        anim.SetBool("isGrounded", !isJumping);
        if (!isDashing)
        {
            anim.SetFloat("horizontalMoveRaw", playerInput.HorizontalMoveRaw);
            anim.SetFloat("verticalMoveRaw", playerInput.VerticalMoveRaw);
        }
    }
    void HandleDash()
    {
        dashTimer -= Time.deltaTime;
        if (isJumping)
            return;
        // Start the Dash if the conditions are met.
        if (playerInput.DashInput && !isDashing && isMoving)
            OnStartDash();

        // End the Dash when the dashDuration is over and reset the parameters.
        else if (Time.time - dashStartTime >= dashDuration && isDashing)
            OnEndDash();            
    }
    void OnStartDash()
    {
        if (dashTimer > 0)
            return;

        //Start the visual effects
        dashVolume.weight = .4f;
        anim.SetTrigger("dash");
        effects[0].gameObject.SetActive(true);
        effects[0].Play();

        // Set the parameters and add force to the player at corresponding direction.
        isDashing = true;
        dashStartTime = Time.time;
        dashTimer = dashCooldown;
        rb.AddForce(transform.forward * dashSpeed, ForceMode.VelocityChange);
    }
    void OnEndDash()
    {
        isDashing = false;
        dashVolume.weight = 0f;
        rb.velocity = Vector3.zero;
        effects[0].gameObject.SetActive(false);
    }
    void Movement()
    {        
        if (isDashing || !canMove)
            return;
        //Finding the Input Direction.
        Vector3 direction = new Vector3(playerInput.HorizontalMove, 0f, playerInput.VerticalMove).normalized;
        Vector3 moveDirection;
        //Calculating target angle and smoothingly rotating the player to target angle considering where the camera is looking at.
        if (isMoving)
        {
            //Calculate and smooth the rotation angle.
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothVelocity, rotationSpeed);
            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

            //Seting the movement direction. 
            moveDirection = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized * movementSpeed * Time.fixedDeltaTime;
            rb.MovePosition(transform.position + moveDirection);
        }
        
    }
    void Jump()
    {
        if (isDashing)
            return;
        // If conditions are met, play the jump animation and add force to player on vertical axis.
        if(playerInput.JumpInput & !isJumping)
        {
            isJumping = true;
            anim.SetTrigger("jump");
            Vector3 jumpForce = new Vector3(0f, jumpSpeed * Time.fixedDeltaTime, 0f);
            rb.AddForce(jumpForce, ForceMode.VelocityChange);
        }

        //If the player started falling, increase the gravity.
        if(rb.velocity.y <=0 && isJumping)
            Physics.gravity = new Vector3(0f, defaultGravity * gravityMultiplier, 0f);
    }
    void MeleeAttack()
    {
        if (playerInput.AttackInput)
        {
            if(comboCount > 0)
            {
                comboTimerOn = true;
            }
            else
            {
                canMove = false;
                rb.velocity = Vector3.zero;
                anim.SetTrigger("meleeAttack");
                comboCount++;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        // If player is touching the ground, set isJumping parameter to false
        if (collision.collider.CompareTag("Ground"))
        {
            isJumping = false;
            Physics.gravity = new Vector3(0f, defaultGravity, 0f);
        }
            
    }

}
