using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryKnight_Attack1 : StateMachineBehaviour
{
 
    public float delay = 0.1f;
    public int attackDamage;
    private float delaytime;
    PlayerMovement_MysteryKnight playermove;
    PlayerCombatScript combat;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        combat = animator.GetComponent<PlayerCombatScript>();
        playermove = animator.GetComponent<PlayerMovement_MysteryKnight>();
        animator.SetBool("continueCombo", false);
        delaytime = delay + 0.2f;
        playermove.animationLocked = true;
        combat.Attack(delay, attackDamage);
        
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        delaytime -= Time.deltaTime;
        if (Input.GetButton("Attack") && delaytime < 0 && combat.stamina > combat.attackStaminaCost)
        {
            animator.SetBool("continueCombo", true);
        }
    }
    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playermove.currentState = "MysteryKnight_Idle";
        playermove.animationLocked = false;
    }

}
