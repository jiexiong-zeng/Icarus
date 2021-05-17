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

    float attackDamageTime = 0f;
    public float damagedelay = 0.25f;
    bool attacked = false;

    // Update is called once per frame
    void Update()
    {


        if (Time.time >= nextAttackTime)
        {
            Playermove.Attacking = false;
            if (Input.GetButtonDown("Attack"))
            {
                animator.SetTrigger("Attack");
                //Attack();
                attacked = false;
                Playermove.Attacking = true;
                nextAttackTime = Time.time + 1f / attackRate;
                attackDamageTime = Time.time + damagedelay;
            }
        }
        if (Time.time >= attackDamageTime && Playermove.Attacking == true && !attacked)
        {
            Attack();
            attacked = true;
        }
    }

    void Attack()
    {
        //animator.SetTrigger("Attack");
       
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }

    }


    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }


}




