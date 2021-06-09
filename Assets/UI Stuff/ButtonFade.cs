using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonFade : MonoBehaviour
{
    public Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        StartCoroutine(fade());
    }

    IEnumerator fade()
    {
        for (float i = 0f; i <= 300; i++)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, i/300);
            yield return new WaitForSeconds(0.01f);
        }
    }

}
