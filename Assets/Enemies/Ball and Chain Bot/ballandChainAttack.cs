using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballandChainAttack : StateMachineBehaviour
{
    public float delay = 0.5f;
    public int attackDamage;

    BallandChainBotController chainmove;
    EnemyCombat combat;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        combat = animator.GetComponent<EnemyCombat>();
        chainmove = animator.GetComponent<BallandChainBotController>();
        combat.Attack(0.05f, attackDamage, "box");
        combat.Attack(delay, attackDamage, "box", 3f);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        chainmove.animationLocked = false;
        chainmove.currentState = "sci ball and chain_idle";
    }
}

