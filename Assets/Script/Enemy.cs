using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public Animator animator;
    public aggro_ctrl test;


    public int maxHealth = 100;
    public int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("Hurt");
        //animator.SetBool("Aggroed", true);
        //Debug.Log("aggro on");
        test.aggroed = true;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetBool("Dead", true);
        //To-do add this to animation controller
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }

    public void Phase()
    {
       GameObject a = animator.gameObject;
       Collider2D[] b = a.GetComponentsInChildren<Collider2D>();
       foreach (var collider in b)
       {
           collider.enabled = false;
       }
    }

    void Update()
    {
        
    }
}
