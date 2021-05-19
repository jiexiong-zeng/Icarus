using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scythe_move : StateMachineBehaviour
{
    public float speed = 2.5f;
    public float attackRange = 0.7f;

    Transform player;
    Rigidbody2D pos;
    SpriteRenderer Sprite;
    Vector3 ColliderPos;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        pos = animator.GetComponentInParent<Rigidbody2D>();
        ColliderPos = animator.transform.GetChild(0).position;
        Sprite = animator.GetComponent<SpriteRenderer>();
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // //If close enough attack
        // // if (Vector2.Distance(player.position,pos.position) <= attackRange)
        // // {
        // //     animator.SetTrigger("Attack");
        // // }
        // // else
        // // {
        // //     //Get vector between the two points
        // //     Debug.Log("range: " + Vector2.Distance(player.position, pos.position));
        // //     Vector2 travel = new Vector2(player.position.x - ColliderPos.x , 0);   
        // //     travel = Vector3.Normalize(travel); 
        // //     Debug.Log(travel);
        // //     //Translate the transform along vector
        // //     pos.MovePosition(pos.position + speed * Time.fixedDeltaTime * travel);
        // // }

        // Vector2 target = new Vector2(player.position.x, pos.position.y);
        // Vector2 newPos = Vector2.MoveTowards(pos.position, target, speed * Time.fixedDeltaTime);
        // pos.MovePosition(newPos);

        // //Debug.Log("range: " + Vector2.Distance(player.position, pos.position));
        // if (Vector2.Distance(player.position,pos.position) <= attackRange)
        // {
        //     animator.SetTrigger("Attack");
        // }

        if (Vector2.Distance(player.position,pos.position) <= attackRange)
        {
            animator.SetTrigger("Attack");
        }

        else
        {
            // //Get vector between the two points
            // Vector3 travel = new Vector3(player.position.x - rb.position.x , 0 , 0);   
            // travel = Vector3.Normalize(travel); 
            // //Translate the transform along vector
            // transform.parent.Translate(travel* speed * Time.deltaTime);

            Vector2 target = new Vector2(player.position.x, pos.position.y);
            Vector2 newPos = Vector2.MoveTowards(pos.position, target, speed * Time.fixedDeltaTime);
            pos.MovePosition(newPos);
        }

    }
}
