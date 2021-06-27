using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryKnight_Parry : StateMachineBehaviour
{
    public float delay = 0.4f;
    public float duration = 0.2f;
    //public int attackDamage;
    //public float forwardMotion = 0.1f;
    //public float attackStaminaCost = 30;
    //private float delaytime;
    private PlayerMovement_MysteryKnight playermove;
    private PlayerCombatScript combat;
    //private bool flashed;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        combat = animator.GetComponent<PlayerCombatScript>();
        playermove = animator.GetComponent<PlayerMovement_MysteryKnight>();
        playermove.animationLocked = true;
        combat.inParry = true;
        combat.Parry(delay,duration);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        /*delaytime -= Time.deltaTime;
        if (Input.GetButton("Primary") && delaytime < 0)
        {
            if (combat.stamina > attackStaminaCost)
            {
                animator.SetBool("continueCombo", true);
            }
            else if (!flashed)
            {
                combat.FlashStaminaBar();
                flashed = true;
            }
        }*/


    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        combat.inParry = false;
        playermove.currentState = "MysteryKnight_Idle";
        playermove.animationLocked = false;
    }

}
