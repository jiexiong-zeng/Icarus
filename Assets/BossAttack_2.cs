using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack_2 : StateMachineBehaviour
{
    public float delay = 0.3f;
    public int attackDamage;
    public float knockback;

    BossController move;
    BossCombat combat;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        combat = animator.GetComponent<BossCombat>();
        move = animator.GetComponent<BossController>();
        Transform playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        if (Vector2.Distance(playerPos.position, animator.GetComponent<Rigidbody2D>().position) <= move.attackRange + 0.1f)
        {
            move.animationLocked = true;
            combat.Attack(delay, attackDamage, knockback);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        move.animationLocked = false;
        //move.currentState = "idle";
    }
}
