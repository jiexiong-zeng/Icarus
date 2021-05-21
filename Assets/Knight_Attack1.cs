using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight_Attack1 : StateMachineBehaviour
{
    //ReferencedScript refScript = GetComponent<ReferencedScript>();
    //public GameObject player;
    public float delay = 0.1f;
    float delaytime;
    public int attackDamage;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerCombatScript combat = animator.GetComponent<PlayerCombatScript>();
        animator.SetBool("Attack1", false);
        delaytime = delay + 0.2f;
        combat.Attack(delay, attackDamage);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log("Attack2 called");
        delaytime -= Time.deltaTime;
        if (Input.GetButton("Attack") && delaytime < 0)
        {
            animator.SetBool("Attack2",true);
        }
    }
    
  
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //PlayerCombatScript combat = animator.GetComponent<PlayerCombatScript>();
        //combat.buffertime = combat.combobuffer;
        PlayerMovement playermove = animator.GetComponent<PlayerMovement>();
        playermove.attacking = false;
        //animator.SetBool("Attack1",false);
    }

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
