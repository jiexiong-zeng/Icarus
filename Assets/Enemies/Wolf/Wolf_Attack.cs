using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf_Attack : StateMachineBehaviour
{

    public float delay = 0.1f;
    public int attackDamage;

    WolfController wolfmove;
    EnemyCombat combat;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        combat = animator.GetComponent<EnemyCombat>();
        wolfmove = animator.GetComponent<WolfController>();
        wolfmove.animationLocked = true;
        combat.Attack(delay, attackDamage);

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        wolfmove.animationLocked = false;
        wolfmove.currentState = "Wolf_Idle";
    }
}
