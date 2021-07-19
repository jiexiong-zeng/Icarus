using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterExplosion : MonoBehaviour
{
    public int damage = 60;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {

            EnemyController enemyMove = other.gameObject.GetComponent<EnemyController>();
            enemyMove.pushedBack = true;
            enemyMove.pushBackDirection = new Vector3(-transform.localScale.x, 0, 0);
            enemyMove.pushBackSpeed = 5;
            //other.transform.position += hitVector * 0.2f;
            other.gameObject.GetComponent<EnemyCombat>().TakeDamage(damage);
            Physics2D.IgnoreCollision(this.GetComponent<CapsuleCollider2D>(), other.GetComponent<CapsuleCollider2D>());

            //other.attachedRigidbody.AddForce(hitVector * 1000);
        }

    }
}
