using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombatScript : MonoBehaviour
{

    public Animator animator;
    public PlayerMovement Playermove;
    public Transform attackPoint;
    public LayerMask enemyLayers;


    //UI
    public GameObject HealthBar;
    public GameObject attackImage;

    public float attackRange = 0.5f;
    public int maxHealth = 100;
    public int currentHealth;

    void Start()
    {
        attackImage = GameObject.Find("Basic Attack");
        currentHealth = maxHealth;
    }

    public void Attack(float delay, int attackDamage)
    {
        //HealthBar.GetComponent<HpBar>().gainHealth(30);
        StartCoroutine(AttackRoutine(delay, attackDamage));
    }

    public IEnumerator AttackRoutine(float delay, int attackDamage)
    {
        //Playermove.animationLocked = true;
        attackImage.GetComponent<Image>().color = Color.red;
        Debug.Log("Delay1");
        yield return new WaitForSeconds(delay);
        attackImage.GetComponent<Image>().color = Color.white;
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
        HealthBar.GetComponent<HpBar>().dropHealth(damage);
    }

        
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }


}




