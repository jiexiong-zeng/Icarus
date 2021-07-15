using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : MonoBehaviour
{
    public float delay;
    public int damage;

    PlayerCombatScript combatScript;
    Collider2D hitbox;

    // Start is called before the first frame update
    void Start()
    {
        hitbox = GetComponent<Collider2D>();
        combatScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombatScript>();
        hitbox.enabled = false;
        StartCoroutine(DamageRoutine());
    }

    IEnumerator DamageRoutine()
    {
        yield return new WaitForSeconds(delay);
        hitbox.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            combatScript.TakeDamage(damage);
        hitbox.enabled = false;
    }

}
