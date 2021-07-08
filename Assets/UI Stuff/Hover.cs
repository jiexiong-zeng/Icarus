using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hover : MonoBehaviour
{
    RectTransform myRect;
    public ObeliskData obelisk;

    // Start is called before the first frame update
    void Start()
    {
        myRect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(myRect, Input.mousePosition))
        {
            GetComponent<Image>().color = new Color(0, 0, 0);
            if (Input.GetMouseButtonDown(0))
            {
                transform.parent.GetComponent<TravelOptions>().CloseOptions(obelisk);
            }
        }
        else
            GetComponent<Image>().color = new Color(255, 255, 255);
    }
}
