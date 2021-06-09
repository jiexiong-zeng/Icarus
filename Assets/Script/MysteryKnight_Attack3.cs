using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryKnight_Attack3 : StateMachineBehaviour
{
    public float delay = 0.1f;
    public int attackDamage;
    PlayerCombatScript combat;
    PlayerMovement_MysteryKnight playermove;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        combat = animator.GetComponent<PlayerCombatScript>();
        playermove = animator.GetComponent<PlayerMovement_MysteryKnight>();
        playermove.animationLocked = true;
        combat.Attack(delay, attackDamage);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playermove.currentState = "MysteryKnight_Idle";
        playermove.animationLocked = false;
    }
}