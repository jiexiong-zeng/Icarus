using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    public static TooltipSystem current;
    public Tooltip tooltip;

    // Start is called before the first frame update
    void Awake()
    {
        if (current == null)
            current = this;
        else
            Destroy(this);
    }

    public static void Show(string content, string header = "")
    {
        current.tooltip.SetText(content, header);
        current.tooltip.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        current.tooltip.gameObject.SetActive(false);
    }

}
