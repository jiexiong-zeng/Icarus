using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_MysteryKnight : MonoBehaviour
{
    private CharacterController2D controller;
    private Animator animator;
    private Rigidbody2D rigidBody;
    private PlayerCombatScript combat;

    private float xAxis;
    private float yAxis;
    public bool attack = false;
    private bool block = true;
    private bool parry = false;
    private bool magic = false;
    private bool jump = false;
    public bool animationLocked = false;
    [SerializeField] private PhysicsMaterial2D noFriction, hasFriction;
    [SerializeField] private float runSpeed = 40f;

    //dash
    [SerializeField] private float dashSpeed = 40f;
    [SerializeField] private float dashDuration = 0.5f;
    private float dashTime = 0f;

    //invul
    public float dashInvulnerableDuration = 0.2f;
    [HideInInspector] public float invulnerableTime = 0f;

    //magic
    public float magicChargeTime = 0.6f;
    private float magicTime;

    //block
    private float blockDelay = 0.1f;
    private float blockTime;
    [SerializeField] private GameObject FireShield;
    private GameObject spawnedShield;

    //Animation
    [HideInInspector] public string currentState;
    const string PLAYER_IDLE = "MysteryKnight_Idle";
    const string PLAYER_RUN = "MysteryKnight_Run";
    const string PLAYER_JUMP = "MysteryKnight_Jump";
    const string PLAYER_FALL = "MysteryKnight_Fall";
    const string PLAYER_DASH = "MysteryKnight_Dash";
    const string PLAYER_MAGIC = "MysteryKnight_Magic";
    const string PLAYER_ATTACK1 = "MysteryKnight_Attack1";
    const string PLAYER_HURT = "MysteryKnight_Hurt";
    const string PLAYER_PARRY= "MysteryKnight_Parry";
    const string PLAYER_COUNTER = "MysteryKnight_Counter";
    const string PLAYER_HEAVYATTACK = "MysteryKnight_HeavyAttack";

    //Unlocks
    private bool airDashUnlocked = true;
    private bool airJumpUnlocked = true;
    private bool hasAirJumped = false;


    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState)
            return;

        animator.Play(newState);

        currentState = newState;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        combat = GetComponent<PlayerCombatScript>();
    }

    private void DashCheck()
    {
        dashTime -= Time.deltaTime;
        invulnerableTime -= Time.deltaTime;
        if (Input.GetButtonDown("Dash") && dashTime < 0 && !animationLocked)
        {
            if (combat.stamina >= combat.dashStaminaCost)
            {
                rigidBody.velocity = new Vector3(rigidBody.velocity.x, 0, 0);
                combat.Dash();
                invulnerableTime = dashInvulnerableDuration;
                if (controller.m_Grounded)
                    dashTime = dashDuration;
                else
                    dashTime = dashDuration + 0.15f;
            }
            else
            {
                combat.FlashStaminaBar();
            }
        }
        
        if(dashTime > 0)
        {
            rigidBody.gravityScale = 0.2f;
        }
        else
        {
            controller.UnFreeze();
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
            else if (magicTime <= magicChargeTime)
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
            combat.AttackRangeNoDelay();
            magicTime = magicChargeTime + 0.3f;
        }
    }


    private void BlockCheck()
    {
        if (block)
        {
            combat.staminaTime = combat.staminaRegenDelay;
            blockTime -= Time.deltaTime;
        }
        else
        {
            blockTime = blockDelay;
            combat.block = false;
            if(spawnedShield)
                spawnedShield.GetComponent<Animator>().SetBool("End", true);
            Destroy(spawnedShield,0.5f);
        }

  
        if (combat.blockBroken)
        {
            blockTime = blockDelay;
            combat.block = false;
            if (spawnedShield)
                spawnedShield.GetComponent<Animator>().SetBool("End", true);
            Destroy(spawnedShield,0.5f);
            combat.blockBroken = false;
        }

        if (blockTime < 0)
        {
            combat.block = true;
            if (spawnedShield)
                spawnedShield.transform.position = transform.position + new Vector3(transform.localScale.x * 0.1f * 3, 0, 0);
        }
    }

    private void StateCheck() // Assortment of status checks
    {
        if (combat.heavyAttack)
        {
            invulnerableTime = 0.2f;
            ChangeAnimationState(PLAYER_HEAVYATTACK);
            combat.heavyAttack = false;
        }
        if (combat.parrysuccess)
        {
            invulnerableTime = 0.5f;
            ChangeAnimationState(PLAYER_COUNTER);
            combat.parrysuccess = false;
        }

        if (dashTime > 0)
        {
            rigidBody.sharedMaterial = noFriction;
        }
        else if (xAxis == 0 || magic || block || animationLocked)
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

        if (controller.m_Grounded)
        {
            hasAirJumped = false;
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
        }

        if (combat.dazed)
        {
            ChangeAnimationState(PLAYER_HURT);
            animationLocked = true;
        }
        else if(currentState == PLAYER_HURT) 
        {
                animationLocked = false;
        }

        if (!controller.m_Grounded && rigidBody.velocity.y < 0 && currentState != PLAYER_HURT && dashTime < 0)
        {
            ChangeAnimationState(PLAYER_FALL);
        }

    }


    void Update()
    {
        //input
        if(!Input.GetButton("Control"))
            xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");
        magic = Input.GetButton("Secondary");
        block = Input.GetButton("Control");

        if (Input.GetButton("Control") && (Input.GetButtonDown("Primary")))
        {
            parry = true;
            if (spawnedShield)
                spawnedShield.GetComponent<Animator>().SetBool("End", true);
            Destroy(spawnedShield, 0.5f);
        }

        else if (Input.GetButtonDown("Primary") && controller.m_Grounded)
            attack = true;

        else if (Input.GetButtonDown("Jump"))
            jump = true;

        DashCheck();
        MagicCheck();
        BlockCheck();
        StateCheck();
    }    

    void FixedUpdate() {
        //if not animation locked
        if (!animationLocked && !combat.dazed)
        {
            //if not grounded
            if (!controller.m_Grounded)
            {
                //controller.Move(xAxis * runSpeed * Time.fixedDeltaTime,false);

                if (airDashUnlocked && dashTime > 0)
                {
                    ChangeAnimationState(PLAYER_DASH);
                    controller.Move(dashSpeed* transform.localScale.x * Time.fixedDeltaTime, false);
                }

                else if(airJumpUnlocked && jump && !hasAirJumped)
                {
                    controller.AirJump();
                    ChangeAnimationState(PLAYER_JUMP);
                    jump = false;
                    hasAirJumped = true;
                }
                else
                {
                    controller.Move(xAxis * runSpeed * Time.fixedDeltaTime, false);
                }

            }

            //if grounded
            if (controller.m_Grounded)// && currentState != PLAYER_JUMP)
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

                else if (parry)
                {
                    controller.Stop();
                    ChangeAnimationState(PLAYER_PARRY);
                    parry = false;
                }

                else if (block)
                {
                    //controller.Stop();
                    ChangeAnimationState(PLAYER_IDLE);
                    if (!spawnedShield)
                        spawnedShield = Instantiate(FireShield, transform.position + new Vector3(transform.localScale.x * 0.1f * 3, 0, 0), Quaternion.identity);
                }
                else if (attack)
                {
                    if (combat.stamina > 30) //custom set to same value
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

                else if (xAxis != 0) //run
                {
                    controller.Move(xAxis * runSpeed * Time.fixedDeltaTime, false);
                    if (!controller.isJumping)
                        ChangeAnimationState(PLAYER_RUN);
                }

                else  //idle
                {
                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName("MysteryKnight_Sheath") && currentState != PLAYER_JUMP)
                        ChangeAnimationState(PLAYER_IDLE);
                }
            }
        }
       
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            invulnerableTime = 0.2f;
            combat.TakeDamage(30);
            Vector3 hitVector = new Vector3((transform.position - other.transform.position).x, 0, 0);
            hitVector = Vector3.Normalize(hitVector);
            controller.Knockback(hitVector.x);
        }
    }

}
