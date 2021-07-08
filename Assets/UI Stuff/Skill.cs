using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    public string skillName;
    public bool isUnlocked;
    public Color displayColor;

    public Color locked;
    public Color unlocked;


    //RectTransform myRect;
    // Start is called before the first frame update
    void Start()
    {
        locked = new Color(0, 0, 0);
        unlocked = new Color(255, 255, 255);
        //myRect = GetComponent<RectTransform>();
        displayColor = isUnlocked ? unlocked : locked;
        GetComponent<Image>().color = displayColor;
    }

    /*
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
    */
}
