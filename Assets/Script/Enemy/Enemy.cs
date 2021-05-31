using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public Animator animator;

    public int maxHealth = 100;
    public int currentHealth;
    public int damage;
    public bool aggroed;

    public Transform attack;
    public float attackRange = 0.2f;

    public GameObject prefab;
    public Transform spawnPoint;

    //Animation control

    //Idle -> Aggro (needs condition (use aggroed bool to play state)) / Hurt -> aggro (auto unless death) (play hurt based on condition it will auto transit) 
    //Attack -> Aggro (auto) (play attack based on conditions)
    //Hurt -> Death (needs condition) (Make seperate) 
    //Aggro -> Idle (use aggro bool) 

    public string idle_state = "";
    public string aggro_state = "";
    public string hurt_state = "";
    public string attack_state = "";   
    public string death_state = "";

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        damage = 30;
        currentHealth = maxHealth;
        aggroed = false;
        //ADD attack through inspector
        //ADD prefab through inspector
        spawnPoint = animator.gameObject.transform.Find("SpawnPoint");

    }

    //Called by player combat  
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        //Set state to hurt
        animator.Play(hurt_state);
        //animator.SetTrigger("Hurt");
        //animator.SetBool("Aggroed", true);
        //Debug.Log("aggro on");
        aggroed = true;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        //Set state to death
        animator.Play(death_state);
        //animator.SetBool("Dead", true);
        //To-do add this to animation controller
        //GetComponent<Collider2D>().enabled = false;
        //enabled = false;
        gameObject.GetComponent<aggro_ctrl>().enabled = false;
        gameObject.layer = 11;
    }

    /*
    public void Phase()
    {
       GameObject a = animator.gameObject;
       Collider2D[] b = a.GetComponentsInChildren<Collider2D>();
       foreach (var collider in b)
       {
           collider.enabled = false;
       }
    }
    */


    private void Update()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).IsName(idle_state) && aggroed)
        {
            animator.Play(aggro_state);
        }
        else if(animator.GetCurrentAnimatorStateInfo(0).IsName(aggro_state) && !aggroed)
        {
            animator.Play(idle_state);
        }
    }


    //Called by movement animation script
    public void DealDamage()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(attack.position, attackRange);
        foreach (Collider2D collider in hitColliders)
        {
            if (collider.tag == "Player")
            {
                collider.GetComponent<PlayerCombatScript>().TakeDamage(damage); //Make sure theres a takedamage func on player side
            }
        }
    }


    public void SpawnProjectile()
    {
        Debug.Log("Spawning at " + spawnPoint.position.x + " " + spawnPoint.position.y + " " + spawnPoint.position.z);
        Instantiate(prefab, spawnPoint.position, transform.rotation);
    }
}
