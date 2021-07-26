using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hover : MonoBehaviour
{
    RectTransform myRect;
    bool isHovering;

    // Start is called before the first frame update
    void Start()
    {
        myRect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(myRect, Input.mousePosition))
            OnHover();
        else
            if (isHovering)
                LeaveHover();
    }

    protected virtual void OnHover()
    {
        isHovering = true;
    }

    protected virtual void LeaveHover()
    {
        isHovering = false;
    }

}


/*
 * 
 * public class Hover : MonoBehaviour
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
            OnHover();
        }
        else
            LeaveHover();
    }

    protected void OnHover()
    {

    }

    protected void LeaveHover()
    {

    }

}
 */ 