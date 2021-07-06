using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    public string skillName;
    RectTransform myRect;
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
            if(Input.GetMouseButtonDown(0))
            {
                Debug.Log("Selected" + skillName);
            }
        }
        else
            GetComponent<Image>().color = new Color(255, 255, 255);
    }

}
