using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crab_attack : StateMachineBehaviour
{

    public float delay = 0.1f;
    public int attackDamage;

    CrabController crabmove;
    EnemyCombat combat;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        combat = animator.GetComponent<EnemyCombat>();
        crabmove = animator.GetComponent<CrabController>();
        crabmove.animationLocked = true;
        combat.Attack(delay, attackDamage);

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        crabmove.animationLocked = false;
        crabmove.currentState = "crab_idle";
    }
}
