using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;
    public Collider2D playerCollider;

    float horizontalMove = 0f;
    public float runSpeed = 40f;

    bool jump = false;
    public float jumpduration = 0.2f;

    public bool attacking;

    public float dashSpeed = 100f;
    bool dash = false;
    public float dashduration = 0.5f;
    float dashtime = 0f;
    float dashDirection = 0f;
    public float invulnerableDuration = 0.2f;
    float invulnerableTime = 0f;
    bool block = false;

    // Update is called once per frame
    void Update()
    {
        playerCollider.enabled = true;

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        dashtime -= Time.deltaTime;
        invulnerableTime -= Time.deltaTime;

        if (!controller.falling && !attacking && controller.m_Grounded)
        {
            if (Input.GetButton("Attack"))
            {
                animator.SetBool("Attack1", true);
                block = false;
                animator.SetBool("Block", false);
            }

            if (Input.GetButton("Block") && !dash && !jump)
            {
                block = true;
                animator.SetBool("Block", true);
            }
            else
            {
                block = false;
                animator.SetBool("Block", false);
            }

            if (Input.GetButtonDown("Jump"))
            {
                block = false;
                jump = true;
                animator.SetBool("Block", false);
                animator.SetBool("Jumping", true);
            }

            if (Input.GetButtonDown("Dash") && !jump && dashtime <= 0)
            {
                block = false;
                animator.SetBool("Block", false);
                dashtime = dashduration;
                invulnerableTime = invulnerableDuration;
                //dashDirection = Input.GetAxisRaw("Horizontal");
            }
            if (dashtime > 0)
            {
                dash = true;
            }
            if (invulnerableTime > 0)
            {
                playerCollider.enabled = false;
            }
          
        }


        if (controller.grab && Input.GetButtonDown("Jump"))
        {
            animator.SetBool("Climb", true);
            //transform.position = new Vector2(transform.position.x, transform.position.y + 0.1f);
        }


    }
    
    
    public void OnLanding ()
    {
    }

    void FixedUpdate()
    {
        animator.SetBool("Roll", false);

        
        if (attacking)
        {
            controller.Stop();
        }
        else if (block)
        {
            controller.Stop();
        }
        else if (dash)
        {
            if (controller.m_FacingRight)
            {
                dashDirection = 1;
            }
            else
            {
                dashDirection = -1;
            }
            controller.Move(dashDirection* dashSpeed* Time.fixedDeltaTime, false, false);
            animator.SetBool("Roll", true);
        }
    
        else
        {
            controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
            
        }
        
        jump = false;
        dash = false;
        //crouch = false;
    }

}
