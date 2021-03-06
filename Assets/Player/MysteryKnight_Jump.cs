using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryKnight_Jump : StateMachineBehaviour
{
    PlayerMovement_MysteryKnight playermove;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playermove = animator.GetComponent<PlayerMovement_MysteryKnight>();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playermove.currentState = "MysteryKnight_Idle";
    }
}
