using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_Attack : StateMachineBehaviour
{

    public float delay = 0.1f;
    public int attackDamage;

    SkeletonController skeletonmove;
    EnemyCombat combat;



    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        combat = animator.GetComponent<EnemyCombat>();
        skeletonmove = animator.GetComponent<SkeletonController>();
        skeletonmove.animationLocked = true;
        combat.Attack(delay, attackDamage);

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        skeletonmove.animationLocked = false;
        skeletonmove.currentState = "Skeleton_Idle";
    }
}
