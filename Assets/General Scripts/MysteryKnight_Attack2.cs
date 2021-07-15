using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryKnight_Attack2 : StateMachineBehaviour
{
   
    public float delay = 0.1f;
    public int attackDamage;
    public float forwardMotion = 0.1f;
    public float attackStaminaCost = 30;
    public float nextAttackStaminaCost = 30;
    private float delaytime;
    PlayerMovement_MysteryKnight playermove;
    PlayerCombatScript combat;
    private bool flashed;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        combat = animator.GetComponent<PlayerCombatScript>();
        playermove = animator.GetComponent<PlayerMovement_MysteryKnight>();
        flashed = false;
        animator.SetBool("continueCombo", false);
        delaytime = delay;
        playermove.animationLocked = true;
        combat.Attack(delay, attackDamage, forwardMotion, attackStaminaCost);
    }
    
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        delaytime -= Time.deltaTime;
        if (Input.GetButtonDown("Primary") && delaytime < 0)
        {
            if (combat.stamina > nextAttackStaminaCost)
            {
                animator.SetBool("continueCombo", true);
            }
            else if (!flashed)
            {
                combat.FlashStaminaBar();
                flashed = true;
            }
        }
    }
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {   
        playermove.currentState = "MysteryKnight_Idle";
        playermove.animationLocked = false;
        playermove.attack = false;
    }

}
