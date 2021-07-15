using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudGuardController : MonoBehaviour
{
    private EnemyController controller;
    private EnemyCombat combat;
    private Rigidbody2D rigidBody;
    private Animator animator;
    private Transform playerPos;

    [SerializeField] private float moveSpeed = 5f;
    [HideInInspector] public bool animationLocked = false;
    [SerializeField] private PhysicsMaterial2D noFriction, hasFriction;
    private bool canMove;

    //Animation
    [HideInInspector] public string currentState = SLEEP;
    const string IDLE = "mudGuard_idle";
    const string RUN = "mudGuard_run";
    const string ATTACK = "mudGuard_attack";
    const string HURT = "mudGuard_idle";
    const string DEATH = "mudGuard_death";
    const string WAKE = "mudGuard_aggro";
    const string SLEEP = "mudGuard_noAggro";

    //aggro
    public bool aggroed = false;
    private float aggroTime;
    [SerializeField] private float aggroDuration = 10f;

    //attack
    private float attackTime;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private float delaybtwAttack = 2f;

    //patrol
    [SerializeField] private bool patrol = false;
    [SerializeField] private Transform m_PatrolPoint1, m_PatrolPoint2;
    private Vector3 patrolPoint1, patrolPoint2, nextPos;
    private float waitTime;
    [SerializeField] private float waitDuration = 2;


    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState && currentState != HURT)
            return;

        animator.Play(newState);

        currentState = newState;
    }

    void Start()
    {
        controller = GetComponent<EnemyController>();
        combat = GetComponent<EnemyCombat>();
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        aggroTime = -aggroDuration;
        moveSpeed = Random.Range(moveSpeed - 1, moveSpeed + 1);
        waitDuration = Random.Range(waitDuration - 1, waitDuration + 1);

        if (patrol)
        {
            patrolPoint1 = m_PatrolPoint1.position;
            patrolPoint2 = m_PatrolPoint2.position;
            nextPos = patrolPoint2;
        }
    }

    void Update()
    {
        aggroTime -= Time.deltaTime;
        waitTime -= Time.deltaTime;
        canMove = StateCheck();
    }

    void FixedUpdate()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        if (canMove)
        {
            Behavior();
        }
    }

    bool StateCheck()
    {

        if (currentState == RUN)
        {
            rigidBody.sharedMaterial = noFriction;
        }
        else
        {
            rigidBody.sharedMaterial = hasFriction;
        }

        if (aggroTime + aggroDuration > Time.time)
        {
            aggroed = true;
        }
        else
        {
            aggroed = false;
        }

        if (combat.dead)
        {
            controller.Freeze();
            if (currentState != DEATH)
                ChangeAnimationState(HURT);
            StartCoroutine(DeathAnimation());
        }

        else if (combat.dazed || combat.damageframe)
        {
            aggroTime = Time.time;
            controller.Stop();
            if (combat.damageframe)
            {
                combat.damageframe = false;
                //ChangeAnimationState(HURT);
            }
        }

        else if (!animationLocked)
        {
            return true;
        }

        return false;

    }
    IEnumerator DeathAnimation()
    {
        yield return new WaitForSeconds(0.2f);
        ChangeAnimationState(DEATH);
    }

    void Behavior()
    {
        if (aggroed)
        {
            CheckFlip();


            if (Vector2.Distance(playerPos.position, rigidBody.position) <= attackRange)
            {

                controller.Stop();

                if (attackTime + delaybtwAttack < Time.time)
                {
                    attackTime = Time.time;
                    ChangeAnimationState(ATTACK);
                }
                else
                {
                    ChangeAnimationState(IDLE);
                }

            }

            else
            {
                if (controller.gap || (Mathf.Abs(playerPos.position.y - rigidBody.position.y) > 0.8f && !controller.isOnSlope))
                {
                    controller.Stop();
                    ChangeAnimationState(IDLE);
                }
                else
                {
                    if (playerPos.position.x - rigidBody.position.x > 0.1f)
                    {
                        ChangeAnimationState(RUN);
                        controller.Move(moveSpeed * Time.deltaTime);
                    }
                    else if (rigidBody.position.x - playerPos.position.x > 0.1f)
                    {
                        ChangeAnimationState(RUN);
                        controller.Move(-moveSpeed * Time.deltaTime);
                    }
                }
            }

        }

        else if (patrol)
        {
                if (waitTime > 0)
                {
                    ChangeAnimationState(IDLE);
                    controller.Stop();
                }
                else
                {
                    if (nextPos == patrolPoint2)
                    {
                        ChangeAnimationState(RUN);
                        controller.Move(moveSpeed * Time.deltaTime);
                    }
                    else
                    {
                        ChangeAnimationState(RUN);
                        controller.Move(-moveSpeed * Time.deltaTime);
                    }
                }

                if (transform.position.x <= patrolPoint1.x && nextPos == patrolPoint1)
                {
                    nextPos = patrolPoint2;
                    if (waitTime < 0)
                        waitTime = waitDuration;
                    //StartCoroutine(Wait());
                }

                if (transform.position.x >= patrolPoint2.x && nextPos == patrolPoint2)
                {
                    nextPos = patrolPoint1;
                    if (waitTime < 0)
                        waitTime = waitDuration;
                    //StartCoroutine(Wait());
                }


        }

        else
        {
                controller.Stop();
                ChangeAnimationState(SLEEP);
        }
        
    }

    void CheckFlip()
    {
        if ((playerPos.position.x > rigidBody.position.x) && !controller.m_FacingRight)
        {
            controller.Stop();
            controller.Flip();
        }

        else if ((rigidBody.position.x > playerPos.position.x) && controller.m_FacingRight)
        {
            controller.Stop();
            controller.Flip();
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {

            if (!aggroed)
            {
                ChangeAnimationState(WAKE);
                combat.dazedtime = Time.time;
            }

            aggroTime = Time.time;

        }
    }


}