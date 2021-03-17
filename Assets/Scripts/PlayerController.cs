using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float dashTimer;
    public float dashCooldown;
    private bool isDashing;
    public float dashSpeed;
    private Animator anim;
    private Rigidbody rb;
    private PlayerInput playerInput;
    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponentInChildren<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
    }
    // Update is called once per frame
    void Update()
    {
        AnimatorHandler();
        Dash();
        Movement();
    }
    void AnimatorHandler()
    {
        anim.SetFloat("horizontalMove", playerInput.HorizontalMove);
        anim.SetFloat("verticalMove", playerInput.VerticalMove);
        if (!isDashing)
        {
            anim.SetFloat("horizontalMoveRaw", playerInput.HorizontalMoveRaw);
            anim.SetFloat("verticalMoveRaw", playerInput.VerticalMoveRaw);
        }
    }
    void Dash()
    {
        dashTimer -= Time.deltaTime;
        if (dashTimer > 0)
            return;
        else isDashing = false;        
        if(playerInput.DashInput)
        {
            isDashing = true;
            anim.SetTrigger("dash");
            dashTimer = dashCooldown;            
            rb.AddForce(playerInput.DashForce * dashSpeed, ForceMode.VelocityChange);
        }
    }
    void Movement()
    {
        if (isDashing)
            return;
    }
}
