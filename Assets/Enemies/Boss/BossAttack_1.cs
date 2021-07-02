using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack_1 : StateMachineBehaviour
{
    public float delay = 0.3f;
    public int attackDamage;
    public float knockback;

    BossController move;
    EnemyCombat combat;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        combat = animator.GetComponent<EnemyCombat>();
        move = animator.GetComponent<BossController>();
        move.animationLocked = true;
        combat.Attack(delay, attackDamage);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        move.animationLocked = false;
    }
}
