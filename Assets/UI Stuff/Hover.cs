using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Hover : MonoBehaviour
{
    RectTransform myRect;
    public ObeliskData obelisk;
    GameObject screenshot;

    // Start is called before the first frame update
    void Start()
    {
        myRect = GetComponent<RectTransform>();
        screenshot = GameObject.Find("Screenshot");
    }

    // Update is called once per frame
    void Update()
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(myRect, Input.mousePosition))
        {
            //GetComponent<Image>().color = new Color(0, 0, 0);
            GetComponentInChildren<TextMeshProUGUI>().color = Color.cyan;
            Debug.Log(GetComponentInChildren<TextMeshProUGUI>().color);
            //Debug.Log(obelisk.pathToImg);
            //Debug.Log(Resources.Load<Sprite>("Obelisks\\test"));
            screenshot.GetComponent<Image>().sprite = Resources.Load<Sprite>(obelisk.pathToImg);
            if (Input.GetMouseButtonDown(0))
            {
                transform.GetComponentInParent<TravelOptions>().CloseOptions(obelisk);
            }
        }
        else
        {
            //GetComponent<Image>().color = new Color(255, 255, 255);
            GetComponentInChildren<TextMeshProUGUI>().color = new Color(255, 255, 255);
        }
    }
}
