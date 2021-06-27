using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FadeIn : MonoBehaviour
{
    private TextMeshProUGUI text; 
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        StartCoroutine(fade(text.text)); //Call whenever needed
    }

   IEnumerator fade(string areaName)
   {
        text.SetText(areaName);
        for(float i = 0; i < 300; i++)
        {
            if(i <= 100)
                text.color = new Color(text.color.r, text.color.g, text.color.b, i / 100);

            else if(i>200)
                text.color = new Color(text.color.r, text.color.g, text.color.b, (300 - i) / 100);

            yield return new WaitForSeconds(0.01f);
        }
   }
}
