using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballandChainCharge : StateMachineBehaviour
{

    BallandChainBotController chainmove;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        chainmove = animator.GetComponent<BallandChainBotController>();
        chainmove.animationLocked = true;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        chainmove.currentState = "sci ball and chain_attack";
    }
}

