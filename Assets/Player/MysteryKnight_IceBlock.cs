using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryKnight_IceBlock : StateMachineBehaviour
{
    public float distance = 3;
    public float attackStaminaCost = 20;
    public float attackManaCost = 20;

    PlayerCombatScript combat;
    PlayerMovement_MysteryKnight playermove;
    public GameObject IceShield;
    private GameObject effect;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        combat = animator.GetComponent<PlayerCombatScript>();
        playermove = animator.GetComponent<PlayerMovement_MysteryKnight>();
        playermove.animationLocked = true;
        combat.Bash(distance, attackStaminaCost, attackManaCost);
        effect = Instantiate(IceShield, animator.transform.position + new Vector3(animator.transform.localScale.x * 0.1f * 2, 0, 0), Quaternion.identity);
        effect.transform.localScale = new Vector3(animator.transform.localScale.x, 1, 1);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playermove.currentState = "MysteryKnight_Idle";
        playermove.animationLocked = false;
    }
}