using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightFlash : MonoBehaviour
{
    private Light2D myLight;
    public float maxIntensity = 1f;
    public float minIntensity = 0f;
    public float pulseSpeed = 1f; //here, a value of 0.5f would take 2 seconds and a value of 2f would take half a second
    private float targetIntensity = 1f;
    private float currentIntensity;
    int i = 0;
    // Start is called before the first frame update
    void Start()
    {
        targetIntensity = maxIntensity;
        myLight = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        currentIntensity = Mathf.MoveTowards(myLight.intensity, targetIntensity, Time.deltaTime * pulseSpeed);
        if (currentIntensity >= maxIntensity)
        {
            i++;
            currentIntensity = maxIntensity;
            targetIntensity = minIntensity;
        }
        else if (currentIntensity <= minIntensity)
        {
            i++;
            currentIntensity = minIntensity;
            targetIntensity = maxIntensity;
        }
        myLight.intensity = currentIntensity;

        if(i == 2)
        {
            Destroy(gameObject);
        }

    }
}
