using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : Hover
{
    public string header;
    [TextArea(15, 20)]
    public string content;
    /*
    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipSystem.Show(content,header);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.Hide();
    }
    */

    protected override void OnHover()
    {
        base.OnHover();
        TooltipSystem.Show(content, header);
    }

    protected override void LeaveHover()
    {
        base.LeaveHover();
        TooltipSystem.Hide();
    }
    
}
