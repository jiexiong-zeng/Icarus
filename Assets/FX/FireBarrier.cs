using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBarrier : MonoBehaviour
{
    public int damage = 30;
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
            Vector3 hitVector = new Vector3((other.transform.position - transform.position).x, 0, 0);
            hitVector = Vector3.Normalize(hitVector);
            EnemyController enemyMove = other.gameObject.GetComponent<EnemyController>();
            enemyMove.pushedBack = true;
            enemyMove.pushBackDirection = hitVector;
            enemyMove.pushBackSpeed = 7;
            //other.transform.position += hitVector * 0.2f;
            other.gameObject.GetComponent<EnemyCombat>().TakeDamage(damage);
            Physics2D.IgnoreCollision(this.GetComponent<CapsuleCollider2D>(), other.GetComponent<CapsuleCollider2D>());

            //other.attachedRigidbody.AddForce(hitVector * 1000);
        }

    }
}
