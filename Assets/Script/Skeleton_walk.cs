using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_walk : StateMachineBehaviour
{

    public float speed = 2.5f;
    public float attackRange = 3f;

    Transform player;
    Rigidbody2D pos;
    SpriteRenderer Sprite;
    //public GameObject AggroCollider;
    Vector3 ColliderPos;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        pos = animator.GetComponentInParent<Rigidbody2D>();
        //ColliderPos = AggroCollider.transform.position;
        //GameObject.Find("TriggerArea")
        if (animator.transform.GetChild(0))
        {
            Debug.Log("true");
        }
        ColliderPos = animator.transform.GetChild(0).position;
        //ColliderPos = GameObject.Find("TriggerArea").transform.position;
        Sprite = animator.GetComponent<SpriteRenderer>();
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector2 target = new Vector2(player.position.x, pos.position.y);
        Vector2 newPos = Vector2.MoveTowards(pos.position, target, speed * Time.fixedDeltaTime);
        pos.MovePosition(newPos);
        if (player.position.x < pos.position.x)
        {
            Sprite.flipX = true;
            ColliderPos.x = -1.7f;
        }
        else
        {
            Sprite.flipX = false;
            ColliderPos.x = 0f;
        }
        //Debug.Log("range: " + Vector2.Distance(player.position, pos.position));
        if (Vector2.Distance(player.position,pos.position) <= attackRange)
        {
            animator.SetTrigger("Attack");
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
