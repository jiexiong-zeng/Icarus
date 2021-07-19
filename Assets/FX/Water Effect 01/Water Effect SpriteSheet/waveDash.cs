using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waveDash : MonoBehaviour
{
    public int damage = 50;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            EnemyController enemyMove = collision.gameObject.GetComponent<EnemyController>();
            enemyMove.pushedBack = true;
            enemyMove.pushBackDirection = new Vector3(transform.localScale.x, 0, 0);
            enemyMove.pushBackSpeed = 10;
            
            //other.transform.position += hitVector * 0.2f;
            collision.gameObject.GetComponent<EnemyCombat>().TakeDamage(damage);

            //other.attachedRigidbody.AddForce(hitVector * 1000);
        }
    }
}
