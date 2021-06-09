using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoBox : MonoBehaviour
{
    public Image image;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        gameObject.SetActive(false);
    }

   public void ShowInfo()
   {
        gameObject.SetActive(true);
        StartCoroutine(fadeIn());
   }


    IEnumerator fadeIn()
    {
        for (float i = 0f; i <= 500; i++)
        {
            if (i > 400)
                image.color = new Color(image.color.r, image.color.g, image.color.b, (500 - i) / 100);
            else
                image.color = new Color(image.color.r, image.color.g, image.color.b, i / 100);
            yield return new WaitForSeconds(0.01f);
        }

        gameObject.SetActive(false);
    }

}
