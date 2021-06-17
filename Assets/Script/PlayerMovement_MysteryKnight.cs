using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_MysteryKnight : MonoBehaviour
{
    private CharacterController2D controller;
    private Animator animator;
    private Rigidbody2D rigidBody;
    private Collider2D playerHitbox;
    private PlayerCombatScript combat;

    public float runSpeed = 40f;
    public float jumpduration = 0.2f;


    private bool attack;

    public float dashSpeed = 40f;
    public float dashDuration = 0.5f;
    float dashTime = 0f;
    public float invulnerableDuration = 0.2f;
    public float invulnerableTime = 0f;

    private bool magic = false;
    private bool jump = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        combat = GetComponent<PlayerCombatScript>();
        playerHitbox = GetComponent<CapsuleCollider2D>();
    }

    //Animation
    public string currentState;
    const string PLAYER_IDLE = "MysteryKnight_Idle";
    const string PLAYER_RUN = "MysteryKnight_Run";
    const string PLAYER_JUMP = "MysteryKnight_Jump";
    const string PLAYER_FALL = "MysteryKnight_Fall";
    const string PLAYER_DASH = "MysteryKnight_Dash";
    const string PLAYER_MAGIC = "MysteryKnight_Magic";
    const string PLAYER_ATTACK1 = "MysteryKnight_Attack1";

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

    public float magicChargeTime = 0.6f;
    private float magicTime;

    private float debugi;
    private void DashCheck()
    {
        dashTime -= Time.deltaTime;
        invulnerableTime -= Time.deltaTime;
        if (Input.GetButtonDown("Dash") && dashTime < 0)
        {
            if (combat.stamina >= combat.dashStaminaCost)
            {
                combat.Dash();
                debugi += 1;
                Debug.Log(debugi);
                dashTime = dashDuration;
                invulnerableTime = invulnerableDuration;
            }
            else
            {
                combat.FlashStaminaBar();
            }
        }
    }

    private void MagicCheck()
    {
        if (magic)
        {
            if (combat.mana >= combat.manaCost)  // sufficient mana
            {
                magicTime -= Time.deltaTime; // start charging magic
            }
            else
            {
                combat.FlashManaBar();
            }
        }
        else
        {
            magicTime = magicChargeTime; //reset magic charge to full
        }


        if (magicTime < 0)   //if magic fully charged
        {
            combat.mana -= combat.manaCost;
            combat.AttackRangeNoDelay();
            magicTime = magicChargeTime + 0.3f;
        }
    }

    private void StateCheck() // Assortment of status checks
    {
        // Friction Material - to prevent sliding off slanted surfaces
        if (dashTime > 0)
        {
            rigidBody.sharedMaterial = noFriction;
        }
        else if (xAxis == 0 || magic || animationLocked)
        {
            rigidBody.sharedMaterial = hasFriction;
        }
        else
        {
            rigidBody.sharedMaterial = noFriction;
        }

        controller.isJumping = false;
        if (currentState == PLAYER_JUMP)
        {
            controller.isJumping = true;
        }

        if (invulnerableTime > 0)
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy Projectile"), true);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy Blocker"), true);
        }
        else
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy Projectile"), false);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy Blocker"), false);
            playerHitbox.enabled = true;
        }

        if (!controller.m_Grounded && rigidBody.velocity.y < 0)
        {
            ChangeAnimationState(PLAYER_FALL);
        }

    }


    void Update()
    {
        //input

        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");
        magic = Input.GetButton("Secondary");

        if (Input.GetButtonDown("Primary"))
            attack = true;

        if (Input.GetButtonDown("Jump"))
            jump = true;

        DashCheck();
        MagicCheck();
        StateCheck();
    }    

    void FixedUpdate() {
        //if not animation locked
        if (!animationLocked && !combat.dazed)
        {
            //if not grounded
            if (!controller.m_Grounded)
            {
                controller.Move(xAxis * runSpeed * Time.fixedDeltaTime,false);
            }

            //if grounded
            if (controller.m_Grounded)
            {
                if (jump)
                {
                    if (yAxis == -1)    //disable collider to drop through platform
                    {
                        StartCoroutine(controller.FallThrough()); 
                    }
                    else
                    {
                        controller.Move(xAxis * runSpeed * Time.fixedDeltaTime, true);
                        ChangeAnimationState(PLAYER_JUMP);
                    }
                    jump = false;
                }

                else if (dashTime > 0) // if in a dash
                {
                    if (controller.m_FacingRight)
                    {
                        controller.Move(dashSpeed * Time.fixedDeltaTime, false);
                    }
                    else
                    {
                        controller.Move(-1 * dashSpeed * Time.fixedDeltaTime, false);
                    }
                    ChangeAnimationState(PLAYER_DASH);
                }

                else if (attack)
                {
                    if (combat.stamina > combat.attackStaminaCost)
                    {
                        controller.Stop();
                        ChangeAnimationState(PLAYER_ATTACK1);
                    }
                    else
                    {
                        combat.FlashStaminaBar();
                    }
                    attack = false;
                }

                else if (magic)
                {
                    controller.Stop();
                    ChangeAnimationState(PLAYER_MAGIC);
                }


                else if (xAxis != 0)
                {
                    controller.Move(xAxis * runSpeed * Time.fixedDeltaTime, false);
                    if (!controller.isJumping)
                        ChangeAnimationState(PLAYER_RUN);
                }

                else  //idle
                {
                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName("MysteryKnight_Sheath") && !controller.isJumping)
                        ChangeAnimationState(PLAYER_IDLE);
                }
            }
        }
       
    }

    
}
