using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Afterimage : MonoBehaviour
{
    public GameObject image;
    public float delay = 0.2f;
    public float lifetime;
    float delayTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        lifetime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > delayTime)
        {
            //Generate afterimage
            delayTime = Time.time + delay;
            var trail = Instantiate(image, transform.position, transform.rotation);
            trail.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
            trail.transform.localScale = transform.localScale;
            Destroy(trail, lifetime);
        }
    }
}
