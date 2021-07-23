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
            GetComponent<Image>().color = new Color(255, 255, 255, 1);
            //Debug.Log(obelisk.pathToImg);
            //Debug.Log(Resources.Load<Sprite>("Obelisks\\test"));
            GameObject.Find("Screenshot").GetComponent<Image>().sprite = Resources.Load<Sprite>(obelisk.pathToImg);
            if (Input.GetMouseButtonDown(0))
            {
                gameObject.GetComponentInParent<TravelOptions>().CloseOptions(obelisk);
            }
        }
        else
            GetComponent<Image>().color = new Color(255, 255, 255, 0.25f);
    }
}
