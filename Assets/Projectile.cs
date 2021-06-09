using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{ 
    Vector3 playerPos;
    Vector3 target;

    public float speed = 1f;
    public int damage = 10;
    public float fireDelay = 0.5f;
    public float selfDestructDelay = 5f;

    private PlayerCombatScript playerCombat;
    private Vector3 targetVector;
    private float startTime;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerCombat = player.GetComponent<PlayerCombatScript>();
        playerPos = player.transform.position;

        target = new Vector3(playerPos.x, playerPos.y, 0);
        transform.right = target - transform.position;

        targetVector = (transform.position - target).normalized;
        

        //invisible until after delay
        GetComponent<CapsuleCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;


        startTime = Time.time;
        Destroy(gameObject, selfDestructDelay);
    }

    // Update is called once per frame

    private bool hit = false;
    void Update()
    {
        if ((Time.time > startTime + fireDelay) && !hit)
        {
            GetComponent<CapsuleCollider2D>().enabled = true;
            GetComponent<SpriteRenderer>().enabled = true;
            transform.position -= targetVector * speed * Time.deltaTime;
        }
        
    }

    public string endAnimation;

    private void OnTriggerEnter2D (Collider2D other)
    {
        hit = true;
        GetComponent<CapsuleCollider2D>().enabled = false;
        GetComponent<Animator>().Play(endAnimation);


        if (other.tag == "Player")
        {
            Vector3 hitVector = (other.transform.position - transform.position).normalized;
            other.attachedRigidbody.AddForce(hitVector * 5000);
            playerCombat.TakeDamage(damage);
        }

        Debug.Log(other.name);
        Destroy(gameObject,0.5f);

    }
}
