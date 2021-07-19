using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderBirdScript : MonoBehaviour
{

    public int damage = 30;
    public float speed = 2f;
    public GameObject thunderHit;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localScale.x == 1)
        {
            transform.position -= Vector3.left * speed * Time.deltaTime;
        }
        else
        {
            transform.position -= Vector3.right * speed * Time.deltaTime;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        //GetComponent<CapsuleCollider2D>().enabled = false;

        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Vector3 hitVector = new Vector3((other.transform.position - transform.position).x, 0, 0);
            hitVector = Vector3.Normalize(hitVector);
            EnemyController enemyMove = other.gameObject.GetComponent<EnemyController>();
            enemyMove.pushedBack = true;
            enemyMove.pushBackDirection = hitVector;
            enemyMove.pushBackSpeed = 3;
            //other.transform.position += hitVector * 0.2f;
            other.gameObject.GetComponent<EnemyCombat>().TakeDamage(damage);
            //Physics2D.IgnoreCollision(this.GetComponent<CapsuleCollider2D>(), other.GetComponent<CapsuleCollider2D>());
            GameObject hitEffect =  Instantiate(thunderHit, transform.position, Quaternion.identity);
            hitEffect.transform.localScale *= 0.5f;
            Destroy(this.gameObject);
            //other.attachedRigidbody.AddForce(hitVector * 1000);
        }

    }
}