using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight_Attack3 : StateMachineBehaviour
{
    public float delay = 0.1f;
    public int attackDamage;
    PlayerCombatScript combat;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        combat = animator.GetComponent<PlayerCombatScript>();
        combat.Attack(delay, attackDamage,0.1f,30);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerMovement playermove = animator.GetComponent<PlayerMovement>();
        playermove.animationLocked = false;
    }
}