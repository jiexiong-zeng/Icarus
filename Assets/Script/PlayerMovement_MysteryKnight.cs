using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_MysteryKnight : MonoBehaviour
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

    bool magic = false;
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
    const string PLAYER_IDLE = "MysteryKnight_Idle";
    const string PLAYER_RUN = "MysteryKnight_Run";
    const string PLAYER_JUMP = "MysteryKnight_Jump";
    const string PLAYER_FALL = "MysteryKnight_Fall";
    const string PLAYER_DASH = "MysteryKnight_Dash";
    const string PLAYER_MAGIC = "MysteryKnight_Magic";
    const string PLAYER_ATTACK1 = "MysteryKnight_Attack1";
    //const string PLAYER_LEDGECLIMB = "Knight_LedgeClimb";


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
    public PhysicsMaterial2D noFriction, hasFriction;

    void Update()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        if (xAxis == 0 || Input.GetButton("Block") || animationLocked)
        {
            rigidBody.sharedMaterial = hasFriction;
        }
        else
        {
            rigidBody.sharedMaterial = noFriction;
        }


        yAxis = Input.GetAxisRaw("Vertical");

        dashTime -= Time.deltaTime;
        invulnerableTime -= Time.deltaTime;
        if (Input.GetButtonDown("Dash") & dashTime < 0)
        {
            dashTime = dashDuration;
            invulnerableTime = invulnerableDuration;
        }

        magic = Input.GetButton("Block");

        attack = Input.GetButton("Attack");

        if (Input.GetButtonDown("Jump"))
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

        if (!controller.m_Grounded && rigidBody.velocity.y < 0)
        {
            ChangeAnimationState(PLAYER_FALL);
        }


        //if not animation locked
        if (!animationLocked)
        {
            //if not grounded
            if (!controller.m_Grounded)
            {
                if (airMovement)
                {
                    controller.Move(xAxis * runSpeed * Time.fixedDeltaTime);
                }

            }

            //if grounded
            if (controller.m_Grounded)
            {
                if (jump && yAxis == -1)
                {
                    StartCoroutine(controller.FallThrough());
                }

                //jump
                else if (jump)
                {
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
                else if (magic)
                {
                    controller.Stop();
                    ChangeAnimationState(PLAYER_MAGIC);
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
                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName("MysteryKnight_Sheath")) {
                        
                        ChangeAnimationState(PLAYER_IDLE);
                    }
                }
            }
        }
        jump = false;
    }

    
}
