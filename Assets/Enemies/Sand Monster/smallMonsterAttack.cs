using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smallMonsterAttack : StateMachineBehaviour
{
    public float delay = 0.1f;
    public int attackDamage;

    SandMonsterController sandMonstermove;
    EnemyCombat combat;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        combat = animator.GetComponent<EnemyCombat>();
        sandMonstermove = animator.GetComponent<SandMonsterController>();
        sandMonstermove.animationLocked = true;
        combat.Attack(delay, attackDamage);

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        sandMonstermove.animationLocked = false;
        sandMonstermove.currentState = "small monster_idle";
        sandMonstermove.runAway = true;
    }
}
