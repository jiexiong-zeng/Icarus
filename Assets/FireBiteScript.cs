using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBiteScript : MonoBehaviour
{
    public int damage = 30;
    public float speed = 1f;
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
        Debug.Log(other.name);
        if(other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("1");
            other.gameObject.GetComponents<EnemyCombat>()[0].TakeDamage(damage);
            Vector3 hitVector = (other.transform.position - transform.position).normalized;
            other.attachedRigidbody.AddForce(hitVector * 5000);
        }

    }
}
