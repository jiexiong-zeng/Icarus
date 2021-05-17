using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;
    float horizontalMove = 0f;
    public float runSpeed = 40f;
    bool jump = false;
    bool crouch = false;
    public bool Attacking;


    public float jumpbuffer = 0.2f;
    float jumptime;
    // Update is called once per frame
    void Update()
    {
        
       
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        jumptime -= Time.deltaTime;

        if (Input.GetButtonDown("Jump"))
        {
            jumptime = jumpbuffer;
            animator.SetBool("Jumping", true);
        }
        if (jumptime > 0)
        {
            jump = true;
        }


        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        } else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }


    }
    
    
    public void OnLanding ()
    {
        animator.SetBool("Jumping", false);
    }

    void FixedUpdate()
    {
        
        if (Attacking)
        {
            controller.Move(0, false, false);
        }
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        
        jump = false;
        //crouch = false;
    }

}
