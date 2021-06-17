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
    public int manaCost = 50;

    private float staminaRegenDelay = 1f;
    private float staminaTime;


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

    public void Attack(float delay, int attackDamage)
    {
        stamina -= attackStaminaCost;
        if (isHUDon)
            stamBar.DropHealth(stamina / maxStamina);
        staminaTime = staminaRegenDelay;
        StartCoroutine(AttackRoutine(delay, attackDamage));
    }

    public IEnumerator AttackRoutine(float delay, int attackDamage)
    {
        yield return new WaitForSeconds(delay);
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

            Vector3 hitVector = (enemy.transform.position - transform.position).normalized;
            hitVector.y += 0.01f;
            enemy.attachedRigidbody.AddForce(hitVector * 5000);
            if(enemy.GetComponent<EnemyCombat>() != null)
                enemy.GetComponent<EnemyCombat>().TakeDamage(attackDamage);
            else if(enemy.GetComponent<BossCombat>() != null)
                enemy.GetComponent<BossCombat>().TakeDamage(attackDamage);
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
        playermove.invulnerableTime = 1f;
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

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            dazedtime = 0.2f;
            Vector3 hitVector = (transform.position - collider.transform.position).normalized;
            hitVector.y += 0.05f;
            GetComponentInParent<Rigidbody2D>().AddForce(hitVector * 3000);
            TakeDamage(20);
        }
    }




    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }


}




