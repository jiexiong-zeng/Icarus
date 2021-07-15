using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rabbitSoldierController : MonoBehaviour
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
    [HideInInspector] public string currentState;
    const string IDLE = "rabbit soldier_idle";
    const string RUN = "rabbit soldier_run";
    const string SMASH_ATTACK = "rabbit soldier_attack1";
    const string STAB_ATTACK = "rabbit soldier_attack2";
    const string HURT = "rabbit soldier_idle";
    const string DEATH = "rabbit soldier_death";

    //aggro
    [HideInInspector] public bool aggroed = false;
    private float aggroTime;
    [SerializeField] private float aggroDuration = 10f;

    //attack
    public GameObject LightFlash;
    public Vector3 LightFlashOffset;

    private float attackTime;
    [SerializeField] private float delaybtwAttack = 2f;
    [SerializeField] private float attackRange = 0.7f;

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
        attackTime -= Time.deltaTime;
        canMove = StateCheck();
    }

    void FixedUpdate()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        //Debug.Log(playerPos);
        if (canMove)
        {
            if (commitedToShortAttack)
            {
                smashAttackCommit();
            }
            else if (commitedToLongAttack)
            {
                stabAttackCommit();
            }
            else
            {
                Behavior();
            }
        }
    }

    //private GameObject Flash;
    void Behavior()
    {
        if (aggroed)
        {
            if (attackTime > 0)
            {
                return;
            }



            CheckFlip();

            if (Vector2.Distance(playerPos.position, rigidBody.position) <= longAttackCommitRange)
            {

                if (Vector2.Distance(playerPos.position, rigidBody.position) <= shortAttackCommitRange)
                {
                    /*
                    if (!Flash)
                    {
                        Flash = Instantiate(LightFlash, transform.position + new Vector3(transform.localScale.x * LightFlashOffset.x, LightFlashOffset.y, LightFlashOffset.z), Quaternion.identity);
                        Flash.transform.Rotate(new Vector3(0, 0, -20));
                    }
                    */
                    smashAttackCommit();
                }
                else
                {
                    /*
                    if (!Flash)
                    {
                        Flash = Instantiate(LightFlash, transform.position + new Vector3(transform.localScale.x * LightFlashOffset.x, LightFlashOffset.y, LightFlashOffset.z), Quaternion.identity);
                        Flash.transform.Rotate(new Vector3(0, 0, -20));
                    }
                    */
                    stabAttackCommit();
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

        else
        {
            if (patrol)
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
                }

                if (transform.position.x >= patrolPoint2.x && nextPos == patrolPoint2)
                {
                    nextPos = patrolPoint1;
                    if (waitTime < 0)
                        waitTime = waitDuration;
                }


            }

            else
            {
                controller.Stop();
                ChangeAnimationState(IDLE);
            }
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
            if (!aggroed)
                attackTime = 1f;
            aggroed = true;
        }
        else
        {
            aggroed = false;
        }


        if (combat.dead)
        {
            controller.Freeze();
            //if (currentState != DEATH)
            //    ChangeAnimationState(HURT);
            StartCoroutine(DeathAnimation());
            //ChangeAnimationState(DEATH);
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

    private int attackDistance = 0;
    private float shortAttackCommitRange = 0.8f;
    private float longAttackCommitRange = 1f;
    private bool commitedToLongAttack = false;
    private bool commitedToShortAttack = false;
    void smashAttackCommit()
    {
        commitedToShortAttack = true;
        if (Vector2.Distance(playerPos.position, rigidBody.position) <= attackRange || attackDistance > 60)
        {
            ChangeAnimationState(SMASH_ATTACK);
            attackDistance = 0;
            commitedToShortAttack = false;
            attackTime = delaybtwAttack;
        }
        else
        {
            attackDistance++;
            ChangeAnimationState(RUN);
            controller.Move(transform.localScale.x * (moveSpeed + 2) * Time.deltaTime);
        }

    }

    private int numberOfAttacks = 0;
    private int moveCounter = 0;
    void stabAttackCommit()
    {
        commitedToLongAttack = true;

        if (numberOfAttacks < 2)
        {
            if (moveCounter > 2)
            {
                ChangeAnimationState(STAB_ATTACK);
                numberOfAttacks++;
                moveCounter = 0;
            }
            else
            {
                ChangeAnimationState(RUN);
                controller.Move(transform.localScale.x * (moveSpeed) * Time.deltaTime);
                moveCounter++;
            }
        }
        else
        {
            commitedToLongAttack = false;
            numberOfAttacks = 0;
            attackTime = delaybtwAttack;
        }

    }

    void CheckFlip()
    {
        if ((playerPos.position.x > rigidBody.position.x) && !controller.m_FacingRight)
        {
            controller.Stop();
            controller.Flip();
        }

        else if ((playerPos.position.x < rigidBody.position.x) && controller.m_FacingRight)
        {
            controller.Stop();
            controller.Flip();
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            aggroTime = Time.time;
        }

    }

}
