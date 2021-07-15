using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterbug_attack : StateMachineBehaviour
{
    public float delay = 0.1f;
    public int attackDamage;

    waterbugController waterbugmove;
    EnemyCombat combat;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        combat = animator.GetComponent<EnemyCombat>();
        waterbugmove = animator.GetComponent<waterbugController>();
        waterbugmove.animationLocked = true;
        combat.Attack(delay, attackDamage, forwardModifier: -1);

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        waterbugmove.animationLocked = false;
        waterbugmove.currentState = "waterbug_idle";
    }
}

