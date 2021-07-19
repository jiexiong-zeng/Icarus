using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBlastScript : MonoBehaviour
{
    public int damage = 20;
    private Transform playerPos;
    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    /*
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
    }*/


    private void OnTriggerEnter2D(Collider2D other)
    {
        //GetComponent<CapsuleCollider2D>().enabled = false;

        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Vector3 hitVector = new Vector3((other.transform.position - playerPos.position).x, 0, 0);
            hitVector = Vector3.Normalize(hitVector);
            EnemyCombat enemyCombat = other.gameObject.GetComponent<EnemyCombat>();
            EnemyController enemyMove = other.gameObject.GetComponent<EnemyController>();
            enemyMove.pushedBack = true;
            enemyMove.pushBackDirection = hitVector;
            enemyMove.pushBackSpeed = 12;
            enemyCombat.TakeDamage(damage);
            //other.transform.position += new Vector3(0, 0.5f, 0);
            //other.attachedRigidbody.AddForce(hitVector * 1000);
            //other.attachedRigidbody.AddForce(hitVector * 1000);
        }

    }
}