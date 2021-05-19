using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatScript : MonoBehaviour
{

    public Animator animator;
    public PlayerMovement Playermove;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackDamage = 40;

    public float attackRate = 2f;
    float nextAttackTime = 0f;

    //float attackDamageTime = 0f;
    public float damagedelay = 0.25f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Attack"))
        {
            animator.SetBool("Attack1", true);
        }

    }
        /*

            if (Time.time >= nextAttackTime)
        {
            //Playermove.Attacking = false;
            if (Input.GetButtonDown("Attack"))
            {
                animator.SetBool("Attack1", true);
                //attacked = false;
                //Playermove.Attacking = true;
                //nextAttackTime = Time.time + 1f / attackRate;
                //attackDamageTime = Time.time + damagedelay;
            }
        }
        //if (Time.time >= attackDamageTime && Playermove.Attacking == true && !attacked)
        //{
        //    Attack();
        //    attacked = true;
        //}*/
  

    public void Attack(float delay, int attackDamage)
    {
        Playermove.Attacking = true;
        StartCoroutine(DelayAction(delay));
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            Debug.Log("We hit " + enemy.name);
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }

    }

    IEnumerator DelayAction(float delayTime)
    {
        //Wait for the specified delay time before continuing.
        yield return new WaitForSeconds(delayTime);
        //Do the action after the delay time has finished.
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }


}




