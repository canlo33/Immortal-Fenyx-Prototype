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
    private bool canMove = true;

    //Jump Related Variables
    private bool isJumping;
    public float jumpSpeed;

    //Rotation Related Variables
    public float rotationSpeed = 0.1f;
    private float smoothVelocity;
    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        camera = Camera.main.transform;
    }
    // Update is called once per frame
    void Update()
    {
        AnimatorHandler();
        HandleDash();
        Movement();
        Jump();
    }
    void AnimatorHandler()
    {
        anim.SetFloat("horizontalMove", playerInput.HorizontalMove);
        anim.SetFloat("verticalMove", playerInput.VerticalMove);
        isMoving = !(playerInput.HorizontalMove == 0f && playerInput.VerticalMove == 0f);
        anim.SetBool("idle", !isMoving);
        anim.SetBool("run", isMoving);
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
        if (playerInput.DashInput && !isDashing && isMoving)
            OnStartDash();
        else if (Time.time - dashStartTime >= dashDuration && isDashing)
            OnEndDash();            
    }
    void OnStartDash()
    {
        if (dashTimer > 0)
            return;
        isDashing = true;
        dashStartTime = Time.time;
        dashVolume.weight = .4f;
        anim.SetTrigger("dash");
        effects[0].gameObject.SetActive(true);
        effects[0].Play();
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
        if (isDashing && !canMove)
            return;
        //Finding the Input Direction.
        Vector3 direction = new Vector3(playerInput.HorizontalMove, 0f, playerInput.VerticalMove).normalized;
        Vector3 moveDirection;
        //Calculating target angle and smoothingly rotating the player to target angle considering where the camera is looking at.
        if (isMoving)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothVelocity, rotationSpeed);
            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);
            //Seting the movement direction. 
            moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            moveDirection.y = rb.velocity.y;            
        }
        else
            moveDirection = new Vector3(0f, rb.velocity.y, 0f);
        rb.velocity = moveDirection.normalized * movementSpeed;

    }
    void Jump()
    {
        if (isDashing || isJumping)
            return;
        if(playerInput.JumpInput)
        {
            isJumping = true;
            anim.SetTrigger("jump");
            Vector3 jumpForce = new Vector3(0f, jumpSpeed, 0f);
            rb.AddForce(jumpForce, ForceMode.VelocityChange);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
            isJumping = false;
    }

}
