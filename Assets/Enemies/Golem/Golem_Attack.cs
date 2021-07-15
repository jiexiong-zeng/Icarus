using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem_Attack : StateMachineBehaviour
{

    public float delay = 0.1f;
    public int attackDamage;

    GolemController golemmove;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        golemmove = animator.GetComponent<GolemController>();
        golemmove.animationLocked = true;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        golemmove.animationLocked = false;
        golemmove.currentState = "Golem_Idle";
    }
}
