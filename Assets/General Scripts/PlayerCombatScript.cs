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
    private CharacterController2D controller;
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

    public int manaRefillOnHit = 5;
    public int manaRecoveryMax = 20;
    public float manaRecovered = 0;
    public float manaRefillRate = 0.2f;
    public float staminaRefillRate = 0.2f;

    public int dashStaminaCost = 30;
    public int fireballmanaCost = 50;
    public int windblastmanaCost = 30;
    public int thunderballmanaCost = 30;
    public int blinkmanaCost = 20;
    public int waveDashmanaCost = 30;
    public int waterminemanaCost = 40;


    public float staminaRegenDelay = 1f;
    public float staminaTime;
    public float manaRegenDelay = 1f;
    public float manaTime;
    public float blinkCooldown = 1f;
    public float blinkTime;

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
        mana = maxMana;
        controller = GetComponent<CharacterController2D>();
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

    void FixedUpdate()
    {

        if (stamina < maxStamina && staminaTime <= 0)
        {
            ChangeStamina(staminaRefillRate, 0);
        }

        if (mana < maxMana && manaTime <= 0 && manaRecovered < manaRecoveryMax)
        {
            ChangeMana(manaRefillRate, 0);
            manaRecovered += manaRefillRate;
        }

        if (block && mana > 0)
            ChangeMana(-0.2f, manaRegenDelay);
        if (mana < 0)
            mana = 0;
    }

    void Update()
    {
        if (Input.GetButton("Primary"))
            heavyAttackChargeTime -= Time.deltaTime;

        staminaTime -= Time.deltaTime;
        manaTime -= Time.deltaTime;
        blinkTime -= Time.deltaTime;

        if (mana > maxMana)
        {
            mana = maxMana;
        }

        dazedtime -= Time.deltaTime;
        if (dazedtime > 0)
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

    public void ChangeMana(float amount, float delay = 0)
    {
        mana += amount;
        manaTime = delay;
        if(amount < 0)
        {
            manaRecovered = 0; //reset regen
        }

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

    public void ChangeStamina(float amount, float delay = 0)
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

            if (heavyAttackChargeTime <= 0 && SkillWheel.selected != 0)
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
                    ChangeMana(manaRefillOnHit);

                    //Vector3 hitVector = new Vector3((enemy.transform.position - transform.position).x, 0, 0);
                    //hitVector = Vector3.Normalize(hitVector);
                    //enemy.attachedRigidbody.AddForce(hitVector * 5000);
                    //enemy.transform.position += hitVector * 0.1f;

                    /*
                    RaycastHit2D Enemyhit = Physics2D.Raycast(enemy.transform.position, Vector2.right * transform.localScale, 10f, m_WhatIsGround);
                    float pushDistance = 0.1f;
                    if (pushDistance > Enemyhit.distance && Enemyhit.distance != 0)
                        pushDistance = hit.distance;
                    
                    enemy.transform.position += new Vector3(transform.localScale.x * pushDistance, 0, 0);
                    */

                    EnemyController enemyMove = enemy.gameObject.GetComponent<EnemyController>();
                    enemyMove.pushedBack = true;
                    Vector3 hitVector = new Vector3((enemy.transform.position - transform.position).x, 0, 0);
                    hitVector = Vector3.Normalize(hitVector);
                    enemyMove.pushBackDirection = hitVector;
                    enemyMove.pushBackSpeed = 4;


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
        //StartCoroutine(SlowMotion(0.1f));
        yield return new WaitForSeconds(delay);
        ChangeStamina(-attackStaminaCost, staminaRegenDelay);

        //screen shake
        CameraEffects.ShakeOnce(0.1f, 1);

        //raycast max distance to avoid phasing through objects
        RaycastHit2D hit = Physics2D.Raycast(transform.position - new Vector3(0, 0.1f, 0), Vector2.right*transform.localScale,10f, m_WhatIsGround);
        forwardMotion *= 2;
        if (forwardMotion > hit.distance && hit.distance != 0)
            forwardMotion = 0;

        transform.position += new Vector3(transform.localScale.x*forwardMotion, 0, 0); //nudge player forward on attack

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position+ new Vector3(transform.localScale.x * 0.25f, 0, 0), attackRange + 0.5f, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            //refill mana base on damage;
            ChangeMana(manaRefillOnHit);

            EnemyController enemyMove = enemy.gameObject.GetComponent<EnemyController>();
            enemyMove.pushedBack = true;
            Vector3 hitVector = new Vector3((enemy.transform.position - transform.position).x, 0, 0);
            hitVector = Vector3.Normalize(hitVector);
            enemyMove.pushBackDirection = hitVector;
            enemyMove.pushBackSpeed = 6;

            enemy.GetComponent<EnemyCombat>().TakeDamage(attackDamage);
        }
    }

    public void Bash(float distance, float attackStaminaCost, float manaCost = 0)
    {
        //StartCoroutine(SlowMotion(0.1f));
        ChangeStamina(-attackStaminaCost, staminaRegenDelay);
        ChangeMana(-manaCost,manaRegenDelay);

        //screen shake
        CameraEffects.ShakeOnce(0.1f, 1);

        //transform.position += new Vector3(transform.localScale.x * forwardMotion, 0, 0); //nudge player forward on attack


        controller.pushedBack = true;
        controller.pushBackDirection = new Vector3(transform.localScale.x, 0, 0);
        controller.pushBackSpeed = distance;
        
    }


    public void DashAttack(int attackDamage, float distance, float attackStaminaCost, float manaCost = 0)
    {
        //StartCoroutine(SlowMotion(0.1f));
        ChangeStamina(-attackStaminaCost, staminaRegenDelay);
        ChangeMana(-manaCost, manaRegenDelay);

        //screen shake
        CameraEffects.ShakeOnce(0.1f, 1);

        controller.Blink(distance);
        //StartCoroutine(SlowMotion(0.1f));
        /*
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(-attackPoint.position, attackRange + 0.2f, enemyLayers);
        //  Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position - new Vector3(transform.localScale.x * 0.25f, 0, 0), attackRange + 0.2f, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            //refill mana base on damage;
            ChangeMana(manaRefillOnHit);

            EnemyController enemyMove = enemy.gameObject.GetComponent<EnemyController>();
            enemyMove.pushedBack = true;
            Vector3 hitVector = new Vector3((enemy.transform.position - transform.position).x, 0, 0);
            hitVector = Vector3.Normalize(hitVector);
            enemyMove.pushBackDirection = hitVector;
            enemyMove.pushBackSpeed = 7;

            enemy.GetComponent<EnemyCombat>().TakeDamage(attackDamage);
        }*/
    }

    public GameObject Fireball;

    public void CastFireball()
    {
        StartCoroutine(SlowMotion(0.2f));
        ChangeMana(-fireballmanaCost, manaRegenDelay);
        Vector3 spawnPos;
        if (transform.localScale.x == 1)
        {
            spawnPos = transform.position + new Vector3(0.5f, 0, 0);
        }
        else
        {
            spawnPos = transform.position - new Vector3(0.5f, 0, 0);
        }
        GameObject Projectile = Instantiate(Fireball, spawnPos, Quaternion.identity);
        Projectile.transform.localScale = transform.localScale;
    }

    public GameObject WindBlast;

    public void CastWindblast()
    {
        //StartCoroutine(SlowMotion(0.2f));
        ChangeMana(-windblastmanaCost, manaRegenDelay);
        Vector3 spawnPos;
        if (transform.localScale.x == 1)
        {
            spawnPos = transform.position + new Vector3(0.5f, 0, 0);
        }
        else
        {
            spawnPos = transform.position - new Vector3(0.5f, 0, 0);
        }
        GameObject Projectile = Instantiate(WindBlast, spawnPos, Quaternion.identity);
        Projectile.transform.localScale = transform.localScale;
    }




    public GameObject ThunderBall;

    public void CastThunderBall()
    {
        //StartCoroutine(SlowMotion(0.2f));
        ChangeMana(-thunderballmanaCost, manaRegenDelay);
        Vector3 spawnPos;
        if (transform.localScale.x == 1)
        {
            spawnPos = transform.position + new Vector3(0.2f, 0, 0);
        }
        else
        {
            spawnPos = transform.position - new Vector3(0.2f, 0, 0);
        }
        GameObject Projectile = Instantiate(ThunderBall, spawnPos, Quaternion.identity);
        Projectile.transform.localScale = transform.localScale;
    }

    public GameObject WaterMine;

    public void CastWatermine()
    {
        //StartCoroutine(SlowMotion(0.2f));
        ChangeMana(-waterminemanaCost, manaRegenDelay);
        Vector3 spawnPos;
        if (transform.localScale.x == 1)
        {
            spawnPos = transform.position + new Vector3(0.4f, 0.2f, 0);
        }
        else
        {
            spawnPos = transform.position - new Vector3(0.4f, 0.2f, 0);
        }
        GameObject Projectile = Instantiate(WaterMine, spawnPos, Quaternion.identity);
        Projectile.transform.localScale = transform.localScale;
    }


    public void Blink()
    {
        ChangeMana(-blinkmanaCost, manaRegenDelay);
        blinkTime = blinkCooldown;
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

        else if ((block && mana >= damage/3) || iceBlock)
        {
            if(!iceBlock)
                ChangeMana(-damage/3, manaRegenDelay);
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
            if(SkillWheel.selected != 4)
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
    public bool iceBlock = false;
    

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




