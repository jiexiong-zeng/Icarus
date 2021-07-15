using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rabbitSoldier_attack1 : StateMachineBehaviour
{
    public float delay = 0.1f;
    public int attackDamage;

    rabbitSoldierController rabbitSoldiermove;
    EnemyCombat combat;



    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        combat = animator.GetComponent<EnemyCombat>();
        rabbitSoldiermove = animator.GetComponent<rabbitSoldierController>();
        rabbitSoldiermove.animationLocked = true;
        combat.Attack(delay, attackDamage,impactModifier:3);

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rabbitSoldiermove.animationLocked = false;
        rabbitSoldiermove.currentState = "rabbit soldier_idle";
    }
}
