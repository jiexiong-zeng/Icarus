using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionsFadeScript : MonoBehaviour
{
    SpriteRenderer sprite;
    bool hasDisplayed;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        Color color = sprite.color;
        color.a = 0;
        sprite.color = color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player" && !hasDisplayed)
        {
            hasDisplayed = true;
            StartCoroutine(Fade());
        }
    }


    IEnumerator Fade()
    {
        Color color = sprite.color;
        color.a = 0;

        while (color.a < 0.75f)
        {
            color.a += 2f * Time.deltaTime;
            sprite.color = color;
            yield return null;
            //yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(2f);

        while (color.a > 0)
        {
            color.a -= 2f * Time.deltaTime;
            sprite.color = color;
            yield return null;
        }

    }

}
