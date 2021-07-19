using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DitzeGames.Effects;
using Cinemachine;


public class PlayerCombatScript : MonoBehaviour
{

    public Animator animator;
    public Transform attackPoint;
    public LayerMask enemyLayers;
    [SerializeField] private LayerMask m_WhatIsGround,blockerLayer;

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
    //public int attackStaminaCost = 50;
    public int dashStaminaCost = 30;
    public int manaCost = 50;

    public float staminaRegenDelay = 1f;
    public float staminaTime;


    public GameObject floatingText;
    public GameObject bloodParticleEffect;
    public GameObject deathParticleEffect;

    private float dazedtime;
    public bool dazed = false;
    public bool dead = false;


    void Start()
    {
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
        if(Input.GetButton("Primary"))
            heavyAttackChargeTime -= Time.deltaTime;

        staminaTime -= Time.deltaTime;

        if (stamina < maxStamina && staminaTime <= 0)
        {
            ChangeStamina(staminaRefillRate, 0);
        }

        if (mana > maxMana)
        {
            mana = maxMana;
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

        //block checks
        if (inParry)
            blockBroken = true;
    }

    public void FlashManaBar()
    {
        StartCoroutine(manaBar.Flash());
    }
    public void FlashStaminaBar()
    {
        StartCoroutine(stamBar.Flash());
    }

    public void ChangeMana(int amount)
    {
        mana += amount;

        if (isHUDon)
        {
            if (amount > 0)
            {
                manaBar.Gain(mana / maxMana);
            }
            else
            {
                manaBar.Drop(mana / maxMana);
            }
        }
    }

    public void ChangeStamina(float amount, float delay)
    {
        stamina += amount;
        staminaTime = delay;

        if (isHUDon)
        {
            if (amount > 0)
            {
                stamBar.Set(stamina / maxStamina);
            }
            else
            {
                stamBar.Drop(stamina / maxStamina);
            }
        }
    }

    public void Dash()
    {
        ChangeStamina(-dashStaminaCost, staminaRegenDelay);
    }

    private float heavyAttackChargeTime;
    public bool heavyAttack = false;
    float hitDistance;
    public void Attack(float delay, int attackDamage, float forwardMotion, float attackStaminaCost)
    {
        heavyAttackChargeTime = delay;
        //GetComponent<Rigidbody2D>().AddForce(new Vector3(transform.localScale.x * forwardMotion * 5000, 0, 0));
        StartCoroutine(AttackRoutine(delay, attackDamage, forwardMotion, attackStaminaCost));
    }

    public IEnumerator AttackRoutine(float delay, int attackDamage, float forwardMotion, float attackStaminaCost)
    {
        
        yield return new WaitForSeconds(delay);


        if (!dazed)
        {

            if (heavyAttackChargeTime <= 0)
            {
                heavyAttack = true;
                //HeavyAttack(attackDamage,forwardMotion,attackStaminaCost);
            }

            else
            {

                ChangeStamina(-attackStaminaCost, staminaRegenDelay);

                //screen shake
                if (attackDamage > 50)
                    CameraEffects.ShakeOnce(0.1f, 1);
                else
                    CameraEffects.ShakeOnce(0.1f, 1, new Vector3(0.1f, 0.1f, 0));

                //forwardMotion += (Input.GetAxisRaw("Horizontal") * 0.1f); //increases or decreases the forward motion base on movement command
                //GetComponent<Rigidbody2D>().AddForce(new Vector3(transform.localScale.x * forwardMotion* 2000, 0, 0));
                //transform.position += new Vector3(transform.localScale.x * forwardMotion, 0, 0); //nudge player forward on attack


                RaycastHit2D hit = Physics2D.Raycast(transform.position - new Vector3(0, 0.1f, 0), Vector2.right * transform.localScale, 10f, m_WhatIsGround);
                //Debug.Log("forwardMotion: " + forwardMotion + ", maxdistance: " + hit.distance);
                if (forwardMotion > hit.distance && hit.distance != 0)
                    forwardMotion = 0;

                //Debug.Log("FinalforwardMotion: " + forwardMotion);
                transform.position += new Vector3(transform.localScale.x * forwardMotion, 0, 0); //nudge player forward on attack

                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

                foreach (Collider2D enemy in hitEnemies)
                {
                    //refill mana base on damage;
                    ChangeMana(manaRefillRate);

                    //Vector3 hitVector = new Vector3((enemy.transform.position - transform.position).x, 0, 0);
                    //hitVector = Vector3.Normalize(hitVector);
                    //enemy.attachedRigidbody.AddForce(hitVector * 5000);
                    //enemy.transform.position += hitVector * 0.1f;


                    RaycastHit2D Enemyhit = Physics2D.Raycast(enemy.transform.position, Vector2.right * transform.localScale, 10f, m_WhatIsGround);
                    float pushDistance = 0.1f;

                    //Debug.Log("pushdist: 0.3f, maxdistance: " + Enemyhit.distance);

                    if (pushDistance > Enemyhit.distance && Enemyhit.distance != 0)
                        pushDistance = hit.distance;

                    enemy.transform.position += new Vector3(transform.localScale.x * pushDistance, 0, 0);

                    enemy.GetComponent<EnemyCombat>().TakeDamage(attackDamage);
                }
            }
        }
   
    }

    public void HeavyAttack(float delay, int attackDamage, float forwardMotion, float attackStaminaCost)
    {
        //GetComponent<Rigidbody2D>().AddForce(new Vector3(transform.localScale.x * forwardMotion * 5000, 0, 0));
        StartCoroutine(HeavyAttackRoutine(delay, attackDamage, forwardMotion, attackStaminaCost));
    }


    IEnumerator HeavyAttackRoutine(float delay, int attackDamage, float forwardMotion, float attackStaminaCost)
    {
        StartCoroutine(SlowMotion(0.1f));
        //StartCoroutine(SlowMotion(0.15f));
        yield return new WaitForSeconds(delay);
        ChangeStamina(-attackStaminaCost, staminaRegenDelay);

        //screen shake
        CameraEffects.ShakeOnce(0.1f, 1);

        //raycast max distance to avoid phasing through objects :DDDDDDD
        RaycastHit2D hit = Physics2D.Raycast(transform.position - new Vector3(0, 0.1f, 0), Vector2.right*transform.localScale,10f, m_WhatIsGround);
        forwardMotion *= 2;
        Debug.Log("forwardMotion: " + forwardMotion + ", maxdistance: " + hit.distance);
        if (forwardMotion > hit.distance && hit.distance != 0)
            forwardMotion = 0;
        Debug.Log(forwardMotion);

        //forwardMotion += (Input.GetAxisRaw("Horizontal") * 0.1f); //increases or decreases the forward motion base on movement command
        //GetComponent<Rigidbody2D>().AddForce(new Vector3(transform.localScale.x * forwardMotion * 10000, 0, 0));

        transform.position += new Vector3(transform.localScale.x*forwardMotion, 0, 0); //nudge player forward on attack

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position+ new Vector3(transform.localScale.x * 0.25f, 0, 0), attackRange + 0.5f, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            //refill mana base on damage;
            ChangeMana(manaRefillRate);

            RaycastHit2D Enemyhit = Physics2D.Raycast(enemy.transform.position, Vector2.right * transform.localScale, 10f, m_WhatIsGround);
            float pushDistance = 0.3f;

            Debug.Log("pushdist: 0.3f, maxdistance: " + Enemyhit.distance);

            if (pushDistance > Enemyhit.distance && Enemyhit.distance != 0)
                pushDistance = hit.distance;

            enemy.transform.position += new Vector3(transform.localScale.x * pushDistance,0,0);

            //enemy.attachedRigidbody.AddForce(hitVector * 10000);
            enemy.GetComponent<EnemyCombat>().TakeDamage(attackDamage);
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
        StartCoroutine(SlowMotion(0.2f));
        ChangeMana(-manaCost);
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

    public GameObject flashEffect;
    public bool blockBroken;

    public void TakeDamage(int damage)
    { 
        if (parry & !dazed)
        {
            Instantiate(flashEffect, transform.position + new Vector3(transform.localScale.x * 0.1f* 4, 0, 0), Quaternion.identity);
            //StartCoroutine(SlowMotion(0.15f));
            GameObject Text = Instantiate(floatingText, transform.position + new Vector3(transform.localScale.x*0.1f,0,0), Quaternion.identity);
            Text.transform.GetChild(0).GetComponent<TextMeshPro>().text = "PARRY";
            Text.transform.GetChild(0).GetComponent<TextMeshPro>().fontSize = 1.2f;
            parrysuccess = true;
        }

        else if (block && stamina >= damage/2)
        {
            ChangeStamina(-damage/2, staminaRegenDelay);
            //Instantiate(flashEffect, transform.position + new Vector3(transform.localScale.x * 0.1f * 4, 0, 0), Quaternion.identity);
            GameObject Text = Instantiate(floatingText, transform.position + new Vector3(transform.localScale.x * 0.1f, 0.2f, 0), Quaternion.identity);
            Text.transform.GetChild(0).GetComponent<TextMeshPro>().text = "BLOCKED";
        }

        else
        {
            if (block)
            {
                CameraEffects.ShakeOnce(0.1f, 1);
                StartCoroutine(SlowMotion(0.2f));
                blockBroken = true;
                GameObject Text = Instantiate(floatingText, transform.position + new Vector3(transform.localScale.x * 0.1f, 0.3f, 0), Quaternion.identity);
                Text.transform.GetChild(0).GetComponent<TextMeshPro>().text = "BLOCK BROKEN";
                Text.transform.GetChild(0).GetComponent<TextMeshPro>().color = Color.red;
                Text.transform.GetChild(0).GetComponent<TextMeshPro>().fontSize = 1.2f;
            }
            dazedtime = 0.2f;
            //instantiate dmgtext
            GameObject dmgText = Instantiate(floatingText, transform.position, Quaternion.identity);
            //set dmgtext to damage taken
            dmgText.transform.GetChild(0).GetComponent<TextMeshPro>().text = damage.ToString();
            //instantiate blood effect
            Instantiate(bloodParticleEffect, transform.position, Quaternion.identity);
            health -= damage;
            if (isHUDon)
                healthBar.Drop(health / maxHealth);
            if (health <= 0)
                Die();
        }
    }

    IEnumerator SlowMotion(float duration)
    {
        Time.timeScale = 0.1f;
        yield return new WaitForSeconds(duration/10);
        Time.timeScale = 1f;
    }
    
    private void Die()
    {
        Time.timeScale = 0.1f;
        //CinemachineVirtualCamera followCam = GameObject.Find("Cinemachine").GetComponent<CinemachineVirtualCamera>();  //zoom?
        //followCam.m_Lens.OrthographicSize = 1;
        Instantiate(deathParticleEffect, transform.position, Quaternion.identity);
        Instantiate(bloodParticleEffect, transform.position, Quaternion.identity);
        Instantiate(bloodParticleEffect, transform.position, Quaternion.identity);
        Instantiate(bloodParticleEffect, transform.position, Quaternion.identity);
        Instantiate(bloodParticleEffect, transform.position, Quaternion.identity);
        Instantiate(bloodParticleEffect, transform.position, Quaternion.identity);
        Instantiate(bloodParticleEffect, transform.position, Quaternion.identity);
        Instantiate(bloodParticleEffect, transform.position, Quaternion.identity);
        LoadingScreenScript loadingScreen = GameObject.Find("LoadingScreen").GetComponent<LoadingScreenScript>();
        loadingScreen.RespawnSequence();
        Destroy(gameObject);
    }


    public void Parry(float delay,float duration)
    {
        ChangeStamina(-10, staminaRegenDelay);
        StartCoroutine(ParryRoutine(delay,duration));
    }

    private bool parry = false;
    public bool parrysuccess = false;
    public bool inParry = false;
    IEnumerator ParryRoutine(float delay, float duration)
    {
        yield return new WaitForSeconds(delay);
        parry = true;
        yield return new WaitForSeconds(duration);
        parry = false;
    }

    public bool block = false;
    

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




