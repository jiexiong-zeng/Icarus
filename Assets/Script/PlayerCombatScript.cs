using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatScript : MonoBehaviour
{

    public Animator animator;
    public PlayerMovement Playermove;
    public Transform attackPoint;
    public LayerMask enemyLayers;

    public float attackRange = 0.5f;
    public int maxHealth = 100;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void Attack(float delay, int attackDamage)
    {
        StartCoroutine(AttackRoutine(delay, attackDamage));
    }

    public IEnumerator AttackRoutine(float delay, int attackDamage)
    {
       //Playermove.animationLocked = true;
        Debug.Log("Delay1");
        yield return new WaitForSeconds(delay);
        Debug.Log("Delay2");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

       foreach (Collider2D enemy in hitEnemies)
       {
            Debug.Log("We hit " + enemy.name);
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
       }
   
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }


}




