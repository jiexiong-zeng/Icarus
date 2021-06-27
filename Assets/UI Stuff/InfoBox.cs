using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoBox : MonoBehaviour
{
    private Image image;
    //private TextMeshProUGUI toolTipText;
   //private RectTransform backgroundRectTransform;
    // Start is called before the first frame update
    void Start()
    {
        image = transform.Find("background").GetComponent<Image>();
        // = transform.Find("text").GetComponent<TextMeshProUGUI>();
        /*
        if(toolTipText != null)
        {
            Debug.Log("1");
        }
        backgroundRectTransform = transform.Find("background").GetComponent<RectTransform>();
        */
        gameObject.SetActive(false);

    }

   public void ShowInfo()
   {
        gameObject.SetActive(true);
        //toolTipText.SetText(textInput);
        /*
        float paddingSize = 4f;
        Vector2 backgroundsize = new Vector2(toolTipText.preferredWidth + paddingSize * 2f, toolTipText.preferredHeight + paddingSize * 2f);
        backgroundRectTransform.sizeDelta = backgroundsize;*/
   }

    public void HideInfo()
    {
        gameObject.SetActive(false);
        //StartCoroutine(fadeOut());
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

    }

    IEnumerator fadeOut()
    {
        for (float i = 500f; i >= 0; i--)
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
