using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Witch_Attack : StateMachineBehaviour
{
    public float targetDelay = 0.1f;
    public int attackDamage = 10;

    private WitchController witchmove;
    private EnemyCombat combat;
    private Vector3 playerPos;
    public Vector3 target;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        combat = animator.GetComponent<EnemyCombat>();
        witchmove = animator.GetComponent<WitchController>();
        witchmove.animationLocked = true;
        combat.AttackRange(targetDelay);


        playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        target = new Vector3(playerPos.x, playerPos.y, 0);


    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        witchmove.animationLocked = false;
        witchmove.currentState = "Witch_Idle";
    }
}
