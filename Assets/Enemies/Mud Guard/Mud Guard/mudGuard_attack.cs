using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mudGuard_attack : StateMachineBehaviour
{
    public float delay = 0.5f;
    public int attackDamage;

    MudGuardController mudGuardmove;
    EnemyCombat combat;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        combat = animator.GetComponent<EnemyCombat>();
        mudGuardmove = animator.GetComponent<MudGuardController>();
        mudGuardmove.animationLocked = true;
        combat.Attack(0.5f, attackDamage);
        combat.Attack(delay, attackDamage, "box", 2f, new Vector3((1-0.375f),0f,0f));
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        mudGuardmove.animationLocked = false;
        mudGuardmove.currentState = "mudGuard_idle";
    }
}
