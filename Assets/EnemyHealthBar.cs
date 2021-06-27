using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    Vector3 initialLocalScale;
    SpriteRenderer sprite;
    public float healthPercentage = 1f;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        //transform.localScale = new Vector3(0.3f, 0.3f, 1);
        initialLocalScale = transform.localScale;

        StartCoroutine(Fade());
    }



    // Update is called once per frame
    void Update()
    {

        transform.localScale = new Vector3(healthPercentage * initialLocalScale.x , initialLocalScale.y, initialLocalScale.z);
    }

    IEnumerator Fade()
    {
        Color color = sprite.color;
        color.a = 0;

        while (color.a < 1)
        {
            color.a += 2f * Time.deltaTime;
            sprite.color = color;
            yield return null;
        }
    }
}


