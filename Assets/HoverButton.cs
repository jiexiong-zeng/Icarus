using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HoverButton : Hover
{
    public UnityEvent OnHoverEvent;
    public UnityEvent OnClickEvent;
    public Color selectedColor;
    public Color baseColor;

    protected override void OnHover()
    {
        base.OnHover();
        GetComponent<Image>().color = selectedColor;
        OnHoverEvent.Invoke();
        if(Input.GetMouseButton(0))
            OnClickEvent.Invoke();
    }

    protected override void LeaveHover()
    {
        base.LeaveHover();
        GetComponent<Image>().color = baseColor;
    }
}
