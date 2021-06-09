using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scythe_move : StateMachineBehaviour
{
    public float speed = 2.5f;
    public float attackRange = 0.7f;
    public float nextAttackTime = 0f;
    public float delayTime = 1f;


    Transform player;
    Rigidbody2D pos;
    Enemy enemy;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        pos = animator.GetComponent<Rigidbody2D>();
        enemy = animator.GetComponent<Enemy>();
        
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Deal damage to player char if in range
        if (Vector2.Distance(player.position, pos.position) <= attackRange)
        {
            animator.Play("Attack");
            animator.SetTrigger("Hurt");
            //Debug.Log("Yo");
            if (Time.time >= nextAttackTime)
            {
                
                enemy.DealDamage(1,1);
                nextAttackTime = Time.time + delayTime;
            }
        }

        else
        {
            Vector2 target = new Vector2(player.position.x, pos.position.y);
            Vector2 newPos = Vector2.MoveTowards(pos.position, target, speed * Time.fixedDeltaTime);
            pos.MovePosition(newPos);
        }

    }
}
