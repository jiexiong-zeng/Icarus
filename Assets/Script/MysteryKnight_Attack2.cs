using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryKnight_Attack2 : StateMachineBehaviour
{
   
    public float delay = 0.1f;
    public int attackDamage;
    private float delaytime;
    private bool continueCombo;
    PlayerMovement_MysteryKnight playermove;
    PlayerCombatScript combat;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        combat = animator.GetComponent<PlayerCombatScript>();
        playermove = animator.GetComponent<PlayerMovement_MysteryKnight>();
        continueCombo = false;
        delaytime = delay + 0.2f;
        playermove.animationLocked = true;
        combat.Attack(delay, attackDamage);
    }
    
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        delaytime -= Time.deltaTime;
        if (Input.GetButton("Attack") && delaytime < 0)
        {
            continueCombo = true;
        }
    }
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (continueCombo)
        {
            playermove.ChangeAnimationState("MysteryKnight_Attack3");
        }
        else
        {
            playermove.animationLocked = false;
        }
    }

}