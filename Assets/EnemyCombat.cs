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
    private int currentHealth;

    public GameObject floatingText;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void Attack(float delay, int attackDamage)
    {
        if (attackPoint == null)
        {
            return;
        }
        StartCoroutine(AttackRoutine(delay, attackDamage));
    }

    public IEnumerator AttackRoutine(float delay, int attackDamage)
    {
        yield return new WaitForSeconds(delay);
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackColliderRadius, playerLayers);

        if (!dazed)
        {
            foreach (Collider2D player in hitPlayer)
            {
                Vector3 hitVector = (player.transform.position - transform.position).normalized;
                hitVector.y += 0.01f;
                player.attachedRigidbody.AddForce(hitVector * 2000);
                player.GetComponent<PlayerCombatScript>().TakeDamage(attackDamage);
            }
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
            Instantiate(RangeProjectile, transform.position, Quaternion.identity);
        }

    }



    public bool spawnParticle = false;
    public GameObject particleEffect;
    private float dazedDuration = 1f;
    public float dazedtime = -1;
    public bool dazed = false;
    public bool dead = false;

    public void TakeDamage(int damage)
    {

        dazedtime = Time.time;

        currentHealth -= damage;


       GameObject dmgText = Instantiate(floatingText, transform.position, Quaternion.identity);
        dmgText.transform.GetChild(0).GetComponent<TextMeshPro>().text = damage.ToString();



        if (spawnParticle)
        {
            Instantiate(particleEffect, transform.position, Quaternion.identity);
        }

        if (currentHealth < 0)
        {
            dead = true;
            Die();
        }

    }

    void Update()
    {
        if(dazedtime + dazedDuration > Time.time)
        {
            dazed = true;
        }
        else
        {
            dazed = false;
        }
    }


    public void Die()
    {
        GetComponent<CapsuleCollider2D>().enabled = false;
        Destroy(this.gameObject,5);
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackColliderRadius);
    }

}
