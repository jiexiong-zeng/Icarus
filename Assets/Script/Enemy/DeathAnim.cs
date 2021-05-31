using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAnim : StateMachineBehaviour
{
    public GameObject enemy;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
         enemy = animator.gameObject;
         enemy.GetComponent<aggro_ctrl>().enabled = false;
         enemy.layer = 11;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
        enemy.SetActive(false); //Maybe change to destroy
    }

}
