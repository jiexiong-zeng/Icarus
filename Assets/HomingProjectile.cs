using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : MonoBehaviour
{
    public float delayTime;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartRoutine());
    }

    IEnumerator StartRoutine()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(delayTime);
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Debug.Log("Hit");
            collision.gameObject.GetComponent<PlayerCombatScript>().TakeDamage(10);
        }
    }
}
