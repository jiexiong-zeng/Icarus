using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight_Attack1 : StateMachineBehaviour
{
 
    public float delay = 0.1f;
    public int attackDamage;
    private float delaytime;
    private bool continueCombo;
    PlayerMovement playermove;
    PlayerCombatScript combat;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        combat = animator.GetComponent<PlayerCombatScript>();
        playermove = animator.GetComponent<PlayerMovement>();
        continueCombo = false;
        delaytime = delay + 0.2f;
        playermove.animationLocked = true;
        //combat.Attack(delay, attackDamage,0.1f,30);
        
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        delaytime -= Time.deltaTime;
        if (Input.GetButton("Attack") && delaytime < 0)
        {
            continueCombo = true;
        }
    }
    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (continueCombo)
        {
            playermove.ChangeAnimationState("Knight_Attack2");
        }
        else
        {
            PlayerMovement playermove = animator.GetComponent<PlayerMovement>();
            playermove.animationLocked = false;
        }

    }

}
