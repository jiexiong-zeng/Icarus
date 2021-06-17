using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlayerCombatScript : MonoBehaviour
{

    public Animator animator;
    public Transform attackPoint;
    public LayerMask enemyLayers;
    private PlayerMovement_MysteryKnight playermove;

    public float attackRange = 0.5f;

    public float maxHealth = 200f;
    public float maxStamina = 200f;
    public float maxMana = 200f;

    public float health;
    public float stamina;
    public float mana;
    public HpBar healthBar;
    public HpBar manaBar;
    public HpBar stamBar;
    bool isHUDon;

    public int manaRefillRate = 50;
    public float staminaRefillRate = 0.2f;
    public int attackStaminaCost = 50;
    public int dashStaminaCost = 30;
    public int manaCost = 50;

    public float staminaRegenDelay = 1f;
    public float staminaTime;


    public GameObject floatingText;
    public GameObject particleEffect;

    private float dazedtime;
    public bool dazed = false;


    void Start()
    {
        playermove = GetComponent<PlayerMovement_MysteryKnight>();
        health = maxHealth;
        stamina = maxStamina;
        mana = 0;

        isHUDon = GameObject.Find("HPBar") == null ? false : true;
        if (isHUDon)
        {
            healthBar = GameObject.Find("HPBar").GetComponent<HpBar>();
            manaBar = GameObject.Find("Mana").GetComponent<HpBar>();
            stamBar = GameObject.Find("Stamina").GetComponent<HpBar>();
            healthBar.Begin(health, maxHealth);
            manaBar.Begin(mana, maxMana);
            stamBar.Begin(stamina, maxStamina);
        } 
    }

    void Update()
    {
        staminaTime -= Time.deltaTime;

        if (stamina < maxStamina && staminaTime <= 0)
        {
            stamina += staminaRefillRate;
            if (isHUDon)
                stamBar.Set(stamina / maxStamina);
        }

        dazedtime -= Time.deltaTime;
        if(dazedtime > 0)
        {
            dazed = true;
        }
        else
        {
            dazed = false;
        }
    }

    public void FlashManaBar()
    {
        StartCoroutine(manaBar.Flash());
    }
    public void FlashStaminaBar()
    {
        StartCoroutine(stamBar.Flash());
    }

    public void Dash()
    {
        stamina -= dashStaminaCost;
        if (isHUDon)
           stamBar.DropHealth(stamina / maxStamina);
        staminaTime = staminaRegenDelay;
    }

    public void Attack(float delay, int attackDamage)
    {
        StartCoroutine(AttackRoutine(delay, attackDamage));
    }

    public IEnumerator AttackRoutine(float delay, int attackDamage)
    {
        
        yield return new WaitForSeconds(delay);

        stamina -= attackStaminaCost;
        if (isHUDon)
            stamBar.DropHealth(stamina / maxStamina);
        staminaTime = staminaRegenDelay;

        transform.position += new Vector3(transform.localScale.x * 0.1f,0,0); //nudge player forward on attack

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

       foreach (Collider2D enemy in hitEnemies)
       {
            //refill mana base on damage;
            mana += manaRefillRate;
            if(mana > maxMana)
            {
                mana = maxMana;
            }
            if (isHUDon)
                manaBar.GainHealth(mana / maxMana);
            Vector3 hitVector = new Vector3((enemy.transform.position - transform.position).normalized.x, 0, 0);
            enemy.transform.position += hitVector*0.1f;
            enemy.GetComponent<EnemyCombat>().TakeDamage(attackDamage);
            //enemy.attachedRigidbody.transform.position.x += 1;
            //enemy.attachedRigidbody.AddForce(hitVector * 1000);
       }
   
    }


    public GameObject RangeProjectile;
    public void AttackRange(float delay)
    {
        if (RangeProjectile == null)
        {
            return;
        }
        StartCoroutine(AttackRangeRoutine(delay));
    }

    public IEnumerator AttackRangeRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!dazed)
        {
            //Spawn slightly in front of the player
            Vector3 spawnPos;
            if(transform.localScale.x == 1)
            {
                spawnPos = transform.position + new Vector3(0.2f, 0, 0);
            }
            else
            {
                spawnPos = transform.position - new Vector3(0.2f, 0, 0);
            }
            GameObject Projectile = Instantiate(RangeProjectile, spawnPos, Quaternion.identity);
            Projectile.transform.localScale = transform.localScale;
        }

    }

    public void AttackRangeNoDelay()
    {
        if (isHUDon)
            manaBar.DropHealth(mana / maxMana);
        Vector3 spawnPos;
        if (transform.localScale.x == 1)
        {
            spawnPos = transform.position + new Vector3(0.2f, 0, 0);
        }
        else
        {
            spawnPos = transform.position - new Vector3(0.2f, 0, 0);
        }
        GameObject Projectile = Instantiate(RangeProjectile, spawnPos, Quaternion.identity);
        Projectile.transform.localScale = transform.localScale;
    }



    public void TakeDamage(int damage)
    {
        playermove.invulnerableTime = 0.2f;
        //instantiate dmgtext
        GameObject dmgText = Instantiate(floatingText, transform.position, Quaternion.identity);
        //set dmgtext to damage taken
        dmgText.transform.GetChild(0).GetComponent<TextMeshPro>().text = damage.ToString();
        //instantiate blood effect
        Instantiate(particleEffect, transform.position, Quaternion.identity);
        health -= damage;
        if (isHUDon)
            healthBar.DropHealth(health/maxHealth);
    }
    /*
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            dazedtime = 0.2f;
            Vector3 hitVector = new Vector3((transform.position - collider.transform.position).normalized.x,0,0);
            GetComponentInParent<Rigidbody2D>().AddForce(hitVector * 1000);
            TakeDamage(20);
        }
    }*/


    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }


}




