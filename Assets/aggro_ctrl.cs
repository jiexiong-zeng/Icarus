using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aggro_ctrl : MonoBehaviour
{

    public Animator anim;
    public Rigidbody2D rb;
    public Transform player;
    public Transform attack;
    public bool facingRight;
    public bool aggroed;
    public Collider2D aggroRange;
    public float attackRange = 0.7f;
    public float scytheRange = 0.2f;
    public float speed = 2f;
    public float delayTime = 1f;
    public float nextAttackTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        facingRight = true;
        aggroed = false;
        rb = anim.GetComponentInParent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        aggroRange = GetComponent<Collider2D>();
    }

    void Update()
    {
        //Face the player (only if already aggroed)
        if(aggroed)
        {
            if (player.position.x < rb.position.x && facingRight)
            {
                Vector2 scale = transform.parent.localScale;
                scale.x *= -1;
                transform.parent.localScale = scale;
                facingRight = false;
            }
            else if (player.position.x > rb.position.x && !facingRight)
            {
                Vector2 scale = transform.parent.localScale;
                scale.x *= -1;
                transform.parent.localScale = scale;
                facingRight = true;
            }

            if (Vector2.Distance(player.position,rb.position) <= attackRange)
            {
                if (Time.time >= nextAttackTime)
                {
                    DealDamage();
                    nextAttackTime = Time.time + delayTime;
                }
            //     anim.SetTrigger("Attack");
            }

            // else
            // {
            //     // //Get vector between the two points
            //     // Vector3 travel = new Vector3(player.position.x - rb.position.x , 0 , 0);   
            //     // travel = Vector3.Normalize(travel); 
            //     // //Translate the transform along vector
            //     // transform.parent.Translate(travel* speed * Time.deltaTime);

            //     Vector2 target = new Vector2(player.position.x, rb.position.y);
            //     Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
            //     rb.MovePosition(newPos);
            // }
        }

        //Check if within range 
        if(aggroRange.OverlapPoint(player.position)) 
        {  
            //Debug.Log("aggro on");
            anim.SetBool("Aggroed", true);
            aggroed = true;
        }
        else
        {  
            // if(aggroed)
            //     Debug.Log("aggro off");
            anim.SetBool("Aggroed", false);
            aggroed = false;
        }
    }


    // Update is called once per frame
    // void OnTriggerEnter2D(Collider2D collider)
    // {
    //     if (collider.tag == "Player")
    //     {
    //         anim.SetBool("Aggroed", true);
    //         aggroed = true;
    //     }
    // }

    // void OnTriggerExit2D(Collider2D collider)
    // {
    //     if (collider.tag == "Player")
    //     {
    //         anim.SetBool("Aggroed", false);
    //     }
    // }

    public void DealDamage()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(attack.position, scytheRange);
        foreach(Collider2D collider in hitColliders)
        {
            if(collider.tag == "Player")
            {
                collider.GetComponent<PlayerCombatScript>().TakeDamage(10);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attack.position, scytheRange);
    }
}
