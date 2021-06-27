using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startpos;
    public GameObject target;
    public float parallaxEffect = 1f;
    // Start is called before the first frame update
    void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        target = GameObject.FindWithTag("Player");
}

    // Update is called once per frame
    void LateUpdate()
    {
        float temp = (target.transform.position.x * (1.0f - parallaxEffect));
        float dist = (target.transform.position.x * parallaxEffect);
        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        if (temp > startpos + length)
        {
            startpos += length;
        }
        else if (temp < startpos - length)
        {
            startpos -= length;
        }
        else if (temp < startpos) 
        {
            startpos -= length;
        }
    }


    /*

    Transform cam; // Camera reference (of its transform)
    Vector3 previousCamPos;

    public float distanceX; // Distance of the item (z-index based) 
    public float distanceY;

    public float smoothingX = 1f; // Smoothing factor of parrallax effect
    public float smoothingY = 1f;

    void Awake()
    {
        cam = Camera.main.transform;
    }

    void LateUpdate()
    {

        if (distanceX != 0f)
        {
            float parallaxX = (previousCamPos.x - cam.position.x) * distanceX;
            Vector3 backgroundTargetPosX = new Vector3(transform.position.x + parallaxX, transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, backgroundTargetPosX, smoothingX * Time.deltaTime);
        }

        if (distanceY != 0f)
        {
            float parallaxY = (previousCamPos.y - cam.position.y) * distanceY;
            Vector3 backgroundTargetPosY = new Vector3(transform.position.x, transform.position.y + parallaxY, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, backgroundTargetPosY, smoothingY * Time.deltaTime);
        }
        previousCamPos = cam.position;
    }


    */







}
