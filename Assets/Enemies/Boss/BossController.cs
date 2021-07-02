using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossController : MonoBehaviour
{
    private EnemyController controller;
    private EnemyCombat combat;
    private Rigidbody2D rigidBody;
    private Animator animator;
    private Transform playerPos;

    public UnityEvent OnBeginEvent;

    public float moveSpeed = 5f;
    public float attackRange = 0.7f;
    public float attackRangedRange = 3f;

    static bool shouldSpawn = true;

    void Start()
    {
        controller = GetComponent<EnemyController>();
        combat = GetComponent<EnemyCombat>();
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        aggroTime = -aggroDuration;

        gameObject.SetActive(shouldSpawn);
        if (shouldSpawn)
        {
            OnBeginEvent.Invoke();
            StartCoroutine(StartAggro());
        }
    }

    public IEnumerator StartAggro()
    {
        yield return new WaitForSeconds(6f);
        aggroed = true;
    }

    //Animation
    public string currentState;
    const string IDLE = "idle";
    const string RUN = "run";
    const string ATTACK = "attack_p1";
    const string HURT = "hurt";
    const string DEATH = "death";
    const string RANGE = "ranged";
    const string HOMING = "homing";


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
    public float delaybtwProjectile = 10f;
    private float attackTime;
    public float projectileTime;

    public UnityEvent OnDeathEvent;

    void FixedUpdate()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
   
        //Debug.Log("Boss:" + rigidBody.velocity.x);
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
            shouldSpawn = false;
            OnDeathEvent.Invoke();
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
                
                else if (Vector2.Distance(playerPos.position, rigidBody.position) <= attackRangedRange && Vector2.Distance(playerPos.position, rigidBody.position) >= attackRange + 0.5f && attackTime + delaybtwProjectile < Time.time)
                {
                    controller.Stop();
                    attackTime = Time.time;
                    ChangeAnimationState(HOMING);
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
                        if (playerPos.position.x - rigidBody.position.x > 0.4f)
                        {
                            ChangeAnimationState(RUN);
                            controller.Move(moveSpeed * Time.deltaTime);
                        }
                        else if (rigidBody.position.x - playerPos.position.x > 0.4f)
                        {
                            ChangeAnimationState(RUN);
                            controller.Move(-moveSpeed * Time.deltaTime);
                        }
                    }
                }

            }

            else
            {
                controller.Stop();
                ChangeAnimationState(IDLE);
            }

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
