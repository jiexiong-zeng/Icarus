using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController2D controller;
    private Animator animator;
    private Rigidbody2D rigidBody;
    public Collider2D playerHitbox;

    public float runSpeed = 40f;
    public float jumpduration = 0.2f;


    private bool attack;

    public float dashSpeed = 40f;
    public float dashDuration = 0.5f;
    float dashTime = 0f;
    public float invulnerableDuration = 0.2f;
    float invulnerableTime = 0f;

    bool block = false;
    public bool airMovement;

    private bool jump = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController2D>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    //Animation
    private string currentState;
    const string PLAYER_IDLE = "Knight_Idle";
    const string PLAYER_RUN = "Knight_Run";
    const string PLAYER_JUMP = "Knight_Jump";
    const string PLAYER_FALL = "Knight_Fall";
    const string PLAYER_DASH = "Knight_Roll";
    const string PLAYER_BLOCK_IDLE = "Knight_Block_Idle";
    const string PLAYER_ATTACK1 = "Knight_Attack1";
    const string PLAYER_LEDGEHANG = "Knight_LedgeGrab";
    const string PLAYER_LEDGECLIMB = "Knight_LedgeClimb";


    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState)
            return;

        animator.Play(newState);

        currentState = newState;
    }


    private float xAxis;
    private float yAxis;
    public bool animationLocked = false;


    void Update()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");

        dashTime -= Time.deltaTime;
        invulnerableTime -= Time.deltaTime;
        if (Input.GetButtonDown("Dash") & dashTime < 0)
        {
            dashTime = dashDuration;
            invulnerableTime = invulnerableDuration;
        }

        //block = false;
        block = Input.GetButton("Block");

        //attack = false;
        attack = Input.GetButton("Attack");

        if (Input.GetButton("Jump"))
        {
            jump = true;
        }
    }


    void FixedUpdate()
    {
        playerHitbox.enabled = true;
        if (invulnerableTime > 0)
        {
            playerHitbox.enabled = false;
        }


        if (controller.atLadder)
        {
            if(yAxis == 1)
            {
                controller.ClimbLadder(true);
            }

            if (yAxis == -1)
            {
                controller.ClimbLadder(false);
            }
        }


        //if not animation locked
        if (!animationLocked)
        {
            //if not grounded
            if (!controller.m_Grounded)
            {
                if (controller.atLedge && yAxis == 1)
                {
                    animationLocked = true;
                    controller.Freeze();
                    ChangeAnimationState(PLAYER_LEDGECLIMB);
                }

                else if (airMovement && xAxis != 0)
                {
                    controller.Move(xAxis * runSpeed * Time.fixedDeltaTime);
                }

                if (rigidBody.velocity.y < 0)
                {
                    ChangeAnimationState(PLAYER_FALL);
                }


            }

            //if grounded
            if (controller.m_Grounded)
            {
                //jump
                if (jump)
                {
                    Debug.Log("YAGHH");
                    //controller.Move(xAxis * (runSpeed + 10) * Time.fixedDeltaTime);
                    controller.Jump();
                    ChangeAnimationState(PLAYER_JUMP);
                }

                //dash
                else if (dashTime > 0)
                {
                    if (controller.m_FacingRight)
                    {
                        controller.Move(dashSpeed * Time.fixedDeltaTime);
                    }
                    else
                    {
                        controller.Move(-1 * dashSpeed * Time.fixedDeltaTime);
                    }
                    ChangeAnimationState(PLAYER_DASH);

                }

                //attack
                else if (attack)
                {
                    animationLocked = true;
                    controller.Stop();
                    ChangeAnimationState(PLAYER_ATTACK1);
                }

                //block
                else if (block)
                {
                    controller.Stop();
                    ChangeAnimationState(PLAYER_BLOCK_IDLE);
                }


                //run
                else if (xAxis != 0)
                {
                    controller.Move(xAxis * runSpeed * Time.fixedDeltaTime);
                    ChangeAnimationState(PLAYER_RUN);
                }

                //idle
                else
                {
                    controller.Stop();
                    ChangeAnimationState(PLAYER_IDLE);
                }
            }
        }
        jump = false;
    }

    
}

    /*
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


        if (controller.grab)
        {
            if (Input.GetButtonDown("Jump"))
            {
                animator.SetBool("Climb", true);
            }

            if (Input.GetButtonDown("Down"))
            {
                //drop
            }


        }



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
    */