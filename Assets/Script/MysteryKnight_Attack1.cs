using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryKnight_Attack1 : StateMachineBehaviour
{
 
    public float delay = 0.1f;
    public int attackDamage;
    private float delaytime;
    private PlayerMovement_MysteryKnight playermove;
    private PlayerCombatScript combat;
    private bool flashed;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        combat = animator.GetComponent<PlayerCombatScript>();
        playermove = animator.GetComponent<PlayerMovement_MysteryKnight>();
        flashed = false;
        animator.SetBool("continueCombo", false);
        delaytime = delay + 0.2f;
        playermove.animationLocked = true;
        combat.Attack(delay, attackDamage);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        delaytime -= Time.deltaTime;
        if (Input.GetButton("Primary") && delaytime < 0)
        {
            if (combat.stamina > combat.attackStaminaCost)
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
    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playermove.currentState = "MysteryKnight_Idle";
        playermove.animationLocked = false;
    }

}
