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
    public float dashSpeed = 100f;
    bool jump = false;
    bool crouch = false;
    public bool Attacking;


    public float jumpbuffer = 0.2f;
    float jumptime;

    bool dash = false;
    float dashtime = 0f;
    public float dashduration = 0.5f;
    float dashDirection = 0f;
    float invulnerableDuration = 0.2f;
    float invulnerableTime = 0f;
   
    // Update is called once per frame
    void Update()
    {
        playerCollider.enabled = true;

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        jumptime -= Time.deltaTime;
        dashtime -= Time.deltaTime;
        invulnerableTime -= Time.deltaTime;

        if (Input.GetButtonDown("Jump"))
        {
            jumptime = jumpbuffer;
            animator.SetBool("Jumping",true);
        }
        if (jumptime > 0)
        {
            jump = true;
        }


        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        } 
        else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }


        if (Input.GetButtonDown("Dash"))
        {
            dashtime = dashduration;
            invulnerableTime = invulnerableDuration;
            dashDirection = Input.GetAxisRaw("Horizontal");
        }
        if (dashtime > 0)
        {
            dash = true;
        }
        if(invulnerableTime > 0)
        {
            playerCollider.enabled = false;
        }

    }
    
    
    public void OnLanding ()
    {
        //Debug.Log("Fell");
        //animator.SetBool("Falling", false);
    }

    void FixedUpdate()
    {
        animator.SetBool("Roll", false);

        if (Attacking)
        {
            controller.Stop();
        }

        else if (dash)
        {
            controller.Move(dashDirection* dashSpeed* Time.fixedDeltaTime, false, false);
            animator.SetBool("Roll", true);
        }

        else
        {
            controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        }
        
        jump = false;
        dash = false;
        //crouch = false;
    }

}
