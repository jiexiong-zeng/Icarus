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
    public float speed = 2f;
    public float delayTime = 1f;
    public float nextAttackTime = 0f;
    public float scytheRange = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        facingRight = true;
        aggroed = false;
        rb = anim.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            if (collider.tag == "Aggro")
                aggroRange = collider;
        }
        //aggroRange = GetComponentInChildren<Collider2D>(); //Defaults to collider in sythe must fix
    }

    void Update()
    {
        //Face the player (only if already aggroed)
        if(aggroed)
        {
            if (player.position.x < rb.position.x && facingRight)
            {
                Vector2 scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
                facingRight = false;
            }
            else if (player.position.x > rb.position.x && !facingRight)
            {
                Vector2 scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
                facingRight = true;
            }

        }

        //Debug.Log(player.position.x);

        //Check if within range 
        if(aggroRange.OverlapPoint(player.position)) 
        {  
            anim.SetBool("Aggroed", true);
            aggroed = true;
        }
        else
        {  
            anim.SetBool("Aggroed", false);
            aggroed = false;
        }
    }

    

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attack.position, scytheRange);
    }
}
