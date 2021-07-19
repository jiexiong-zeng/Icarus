using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class EnemyCombat : MonoBehaviour
{

    public Transform attackPoint;
    public LayerMask playerLayers;

    public int maxHealth = 100;
    public float attackColliderRadius = 0.7f;
    public Vector2 attackBoxSize = new Vector2(1, 1);
    public int currentHealth;

    public GameObject floatingText;
    private Collider2D[] hitPlayer;

    void Start()
    {
        currentHealth = maxHealth;

    }
    public void Attack(float delay, int attackDamage, string specialHitbox = "null", float impactModifier = 1f, Vector3 hitboxVectorModifier = new Vector3(), int forwardModifier = 1)
    {

        if (attackPoint == null)
        {
            return;
        }
        StartCoroutine(AttackRoutine(delay, attackDamage, specialHitbox, impactModifier, hitboxVectorModifier, forwardModifier));
    }

    public IEnumerator AttackRoutine(float delay, int attackDamage,string specialHitbox, float impactModifier, Vector3 hitboxVectorModifier, int forwardModifier)
    {
        yield return new WaitForSeconds(delay);
        transform.position += new Vector3(transform.localScale.x * 0.1f * forwardModifier, 0, 0); //nudge forward on attack

        if (specialHitbox == "box")
        {
            hitPlayer = Physics2D.OverlapBoxAll(attackPoint.position + transform.localScale.x*hitboxVectorModifier, attackBoxSize, playerLayers);
        }
        else
        {
            hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position + transform.localScale.x*hitboxVectorModifier, attackColliderRadius, playerLayers);
        }

        if (!dazed && !dead)
        {
            foreach (Collider2D player in hitPlayer)
            {
                if (player.GetComponent<PlayerCombatScript>() != null)
                {
                    //Vector3 hitVector = (player.transform.position - transform.position).normalized;
                    //hitVector.y += 0.01f;
                    Vector3 hitVector = new Vector3((player.transform.position - transform.position).x, 0, 0);
                    hitVector = Vector3.Normalize(hitVector);
                    player.attachedRigidbody.AddForce(hitVector*1000* impactModifier);
                    player.transform.position += hitVector * 0.1f;
                    //player.attachedRigidbody.AddForce(hitVector * 2000);
                    player.GetComponent<PlayerCombatScript>().TakeDamage(attackDamage);
                }
            }
        }

    }
    public GameObject RangeProjectile;
    private GameObject SpawnedProjectile;
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
            SpawnedProjectile = Instantiate(RangeProjectile, transform.position, Quaternion.identity);
        }

    }


    public bool canBeDazed = true;
    public bool spawnParticle = false;
    public GameObject bloodEffect;
    public GameObject manaParticle;
    public int manaRecoverAmount = 50;

    [SerializeField] private float dazedDuration = 1f;
    [HideInInspector] public float dazedtime = -1;
    [HideInInspector] public bool dazed = false;
    [HideInInspector] public bool dead = false;
    [HideInInspector] public bool damageframe = false;

    GameObject healthBar;
    public GameObject healthBarPrefab;
    public Vector3 healthBarOffset = new Vector3(-0.2f,0.4f,0);
    public void TakeDamage(int damage)
    {

        dazedtime = Time.time;
        currentHealth -= damage;
        damageframe = true;

        if (!healthBar)
        {

            healthBar = Instantiate(healthBarPrefab, transform.position + healthBarOffset, Quaternion.identity);
        }

        if (healthBar)
        {
            healthBar.GetComponent<EnemyHealthBar>().healthPercentage = (float)currentHealth / maxHealth;
        }


        GameObject dmgText = Instantiate(floatingText, transform.position, Quaternion.identity); 
        dmgText.transform.GetChild(0).GetComponent<TextMeshPro>().text = damage.ToString();

        if (spawnParticle)
        {
            Instantiate(bloodEffect, transform.position, Quaternion.identity); //blood
        }

        if (SpawnedProjectile)
        {
            SpawnedProjectile.GetComponent<Projectile>().destroySelf = true;
        }

        if (currentHealth < 0)
        {
            dead = true;
            Die();
        }

    }

    public GameObject HomingProjectile;
    public Vector3 projectileOffset;
    public void Homing(float delay)
    {
        if(HomingProjectile != null)
            StartCoroutine(HomingRoutine(delay));
    }

    public IEnumerator HomingRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!dazed)
            Instantiate(HomingProjectile, GameObject.FindGameObjectWithTag("Player").transform.position + projectileOffset, Quaternion.identity);
    }


    void Update()
    {
        if(dazedtime + dazedDuration > Time.time && canBeDazed)
        {
            dazed = true;
        }
        else
        {
            dazed = false;
        }

        if (healthBar)
        {
            healthBar.transform.position = transform.position + healthBarOffset;
        }

    }


    public void Die()
    {
        if (healthBar)
        {
            Destroy(healthBar);
        }

        CapsuleCollider2D[] colList = transform.GetComponentsInChildren<CapsuleCollider2D>();
        
        CapsuleCollider2D[] playerColList = GameObject.FindGameObjectWithTag("Player").GetComponentsInChildren<CapsuleCollider2D>();



        foreach (CapsuleCollider2D col in colList)
        {
            foreach(CapsuleCollider2D playerCol in playerColList)
                Physics2D.IgnoreCollision(col, playerCol,true);
            //col.enabled = false;
        }
        Destroy(this.gameObject,10);

        StartCoroutine(SpawnMana(1f));
        
    }
    IEnumerator fade(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        for (float i = 300; i >= 0; i--)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, i / 300);
            yield return new WaitForSeconds(0.01f);
        }
    }
    IEnumerator SpawnMana(float delay)
    {
        yield return new WaitForSeconds(delay);
        Instantiate(manaParticle, transform.position, Quaternion.identity); //mana
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombatScript>().ChangeMana(manaRecoverAmount);
        StartCoroutine(fade(2));
    }


    void OnDrawGizmosSelected()
    {
        if (attackPoint)
        {
            Gizmos.DrawWireSphere(attackPoint.position, attackColliderRadius);
            Gizmos.DrawWireCube(attackPoint.position, attackBoxSize);
        }
    }

}
