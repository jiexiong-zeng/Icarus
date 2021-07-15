using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slime_attack : StateMachineBehaviour
{
    public float delay = 0.1f;
    public int attackDamage;

    SlimeController slimemove;
    EnemyCombat combat;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        combat = animator.GetComponent<EnemyCombat>();
        slimemove = animator.GetComponent<SlimeController>();
        slimemove.animationLocked = true;
        combat.Attack(delay, attackDamage, forwardModifier:-1);

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        slimemove.animationLocked = false;
        slimemove.currentState = "slime_idle";
    }
}
