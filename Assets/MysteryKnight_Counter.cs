using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryKnight_Counter : StateMachineBehaviour
{
    public float delay = 0f;
    public int attackDamage = 50;
    public float forwardMotion = 0.2f;
    public float attackStaminaCost = 30;
    PlayerCombatScript combat;
    PlayerMovement_MysteryKnight playermove;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        combat = animator.GetComponent<PlayerCombatScript>();
        playermove = animator.GetComponent<PlayerMovement_MysteryKnight>();
        playermove.animationLocked = true;
        combat.HeavyAttack(delay, attackDamage, forwardMotion, attackStaminaCost);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playermove.currentState = "MysteryKnight_Idle";
        playermove.animationLocked = false;
    }
}
