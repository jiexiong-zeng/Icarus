using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfController : MonoBehaviour
{
    private EnemyController controller;
    private EnemyCombat combat;
    private Rigidbody2D rigidBody;
    private Animator animator;
    private Transform playerPos;

    public float moveSpeed = 5f;
    public float attackRange = 0.5f;

    void Start()
    {
        controller = GetComponent<EnemyController>();
        combat = GetComponent<EnemyCombat>();
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        aggroTime = -aggroDuration;

        if (patrol)
        {
            patrolPoint1 = m_PatrolPoint1.position;
            patrolPoint2 = m_PatrolPoint2.position;
            nextPos = patrolPoint2;
        }
    }

    //Animation
    public string currentState;
    const string IDLE = "Wolf_Idle";
    const string RUN = "Wolf_Run";
    const string ATTACK = "Wolf_Attack";
    const string HURT = "Wolf_Hurt";
    const string DEATH = "Wolf_Death";


    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState)
            return;

        animator.Play(newState);

        currentState = newState;
    }


    public bool animationLocked = false;
    public PhysicsMaterial2D noFriction, hasFriction;
    private float aggroTime;
    public float aggroDuration = 10f;
    public bool aggroed = false;

    public float delaybtwAttack = 2f;
    private float attackTime;

    public bool patrol = false;
    [SerializeField] private Transform m_PatrolPoint1, m_PatrolPoint2;
    Vector3 patrolPoint1, patrolPoint2, nextPos;


    void FixedUpdate()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    {
        if (currentState == RUN)
        {
            rigidBody.sharedMaterial = noFriction;
        }
        else
        {
            rigidBody.sharedMaterial = hasFriction;
        }

        if (combat.dead)
        {
            controller.Freeze();
            ChangeAnimationState(DEATH);
        }

        else if (combat.dazed)
        {
            aggroTime = Time.time;
            controller.Stop();
            ChangeAnimationState(HURT);
        }

        else if (!animationLocked)
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


                    if (transform.position.x <= patrolPoint1.x && nextPos == patrolPoint1)
                    {
                        nextPos = patrolPoint2;
                        StartCoroutine(Wait());
                    }

                    if (transform.position.x >= patrolPoint2.x && nextPos == patrolPoint2)
                    {
                        nextPos = patrolPoint1;
                        StartCoroutine(Wait());
                    }


                }

                else
                {
                    controller.Stop();
                    ChangeAnimationState(IDLE);
                }
            }

        }

        if(aggroTime + aggroDuration > Time.time)
        {
            aggroed = true;
        }
        else
        {
            aggroed = false;
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




    public GameObject Exclamation;
   
    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {

            if (!aggroed)
            {
                combat.dazedtime = Time.time;
                Instantiate(Exclamation, transform.position, Quaternion.identity);
            }

            aggroTime = Time.time;

        }
    }



    IEnumerator Wait()
    {
        patrol = false;
        yield return new WaitForSeconds(2f);
        patrol = true;
    }


}
