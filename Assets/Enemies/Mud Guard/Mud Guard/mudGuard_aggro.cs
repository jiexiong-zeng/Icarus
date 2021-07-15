using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mudGuard_aggro : StateMachineBehaviour
{
    MudGuardController mudGuardmove;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        mudGuardmove = animator.GetComponent<MudGuardController>();
        mudGuardmove.animationLocked = true;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        mudGuardmove.animationLocked = false;
        mudGuardmove.currentState = "mudGuard_idle";
    }
}
