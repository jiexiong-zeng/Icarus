using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryKnight_DashAttack : StateMachineBehaviour
{
    public int attackDamage = 50;
    public float forwardMotion = 0.7f;
    public float attackStaminaCost = 30;
    public float attackManaCost = 30;

    PlayerCombatScript combat;
    PlayerMovement_MysteryKnight playermove;
    public GameObject DashAttackEffect;
    private GameObject effect;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        combat = animator.GetComponent<PlayerCombatScript>();
        playermove = animator.GetComponent<PlayerMovement_MysteryKnight>();
        playermove.animationLocked = true;
        combat.DashAttack(attackDamage, forwardMotion, attackStaminaCost, attackManaCost);
        effect = Instantiate(DashAttackEffect, animator.transform.position - new Vector3(animator.transform.localScale.x * 0.1f * 5, 0, 0), Quaternion.identity);
        effect.transform.localScale = new Vector3(animator.transform.localScale.x,1,1);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playermove.currentState = "MysteryKnight_Idle";
        playermove.animationLocked = false;
    }
}

