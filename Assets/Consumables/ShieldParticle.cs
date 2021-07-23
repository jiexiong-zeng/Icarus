using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldParticle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var shield = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<ParticleSystem>();
        shield.enableEmission = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
