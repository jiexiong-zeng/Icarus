using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryKnight_Bash : StateMachineBehaviour
{
    //public float delay = 0f;
    public float distance = 10;
    public float attackStaminaCost = 30;
    public float attackManaCost = 30;
    PlayerCombatScript combat;
    PlayerMovement_MysteryKnight playermove;
    public GameObject fireBashEffect;
    private GameObject effect;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        combat = animator.GetComponent<PlayerCombatScript>();
        playermove = animator.GetComponent<PlayerMovement_MysteryKnight>();
        playermove.animationLocked = true;
        combat.Bash(distance, attackStaminaCost, attackManaCost);
        effect = Instantiate(fireBashEffect, animator.transform.position + new Vector3(animator.transform.localScale.x * 0.1f * 2, 0, 0), Quaternion.identity);
        effect.transform.localScale = animator.transform.localScale;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        effect.transform.position = animator.transform.position + new Vector3(animator.transform.localScale.x * 0.1f * 2, 0, 0);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playermove.currentState = "MysteryKnight_Idle";
        playermove.animationLocked = false;
    }
}
