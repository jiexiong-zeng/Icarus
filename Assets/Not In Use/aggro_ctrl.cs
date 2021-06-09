using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aggro_ctrl : MonoBehaviour
{
    private Transform player;
    public bool facingRight;
    private Collider2D aggroRange;
    private Enemy enemy;
   
    // Start is called before the first frame update
    void Start()
    {
        enemy = gameObject.GetComponent<Enemy>();
        facingRight = true;
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
        if(enemy.aggroed)
        {
            if (player.position.x < transform.position.x && facingRight)
            {
                Vector2 scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
                facingRight = false;
            }
            else if (player.position.x > transform.position.x && !facingRight)
            {
                Vector2 scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
                facingRight = true;
            }

        }

        //Check if within range 
        if(aggroRange.OverlapPoint(player.position)) 
        {  
            //anim.Play("Aggroed");
            //anim.SetBool("Aggroed", true);
            enemy.aggroed = true;
        }
        else
        {  
            //anim.Play(idle_state);
            //anim.SetBool("Aggroed", false);
            enemy.aggroed = false;
        }
    }

}
