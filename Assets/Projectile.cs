using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{ 
    Transform player;
    Vector3 direction;
    public float speed = 0.5f;
    //Enemy enemy;
    public int damage = 10;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        direction = Vector3.Normalize(player.position - transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);      
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            player.GetComponent<PlayerCombatScript>().TakeDamage(damage);
            Debug.Log(other.name);
            Destroy(gameObject);
        }
        //enemy.DealDamage();
    }
}
