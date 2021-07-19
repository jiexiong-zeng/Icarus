using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMine : MonoBehaviour
{
    public GameObject waveExplosion;
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

            Instantiate(waveExplosion, transform.position + new Vector3(0,0.35f,0), Quaternion.identity);
            Destroy(this.gameObject);
        }

    }
}
