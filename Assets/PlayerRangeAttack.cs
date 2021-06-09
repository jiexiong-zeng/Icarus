using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRangeAttack : StateMachineBehaviour
{
    public float delay = 0.6f;
    PlayerCombatScript combat;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        combat = animator.GetComponent<PlayerCombatScript>();
        combat.AttackRange(delay);

    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

}
