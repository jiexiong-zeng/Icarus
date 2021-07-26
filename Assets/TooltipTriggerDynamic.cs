using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipTriggerDynamic : Hover
{
    public string header;
    [TextArea(15, 20)]
    public string content;
    public int offset;

    void GetContent()
    {
        if(InventoryManager.activeSlot == -9999)
        {
           //leave as it is, base values in inspector
        }

        else
        {
            var pos = InventoryManager.activeSlot + offset;
            if (pos < 0)
                pos = InventoryManager.inventory.Count - 1;
            pos = pos % InventoryManager.inventory.Count;
            header = InventoryManager.inventory[pos].name;
            content = InventoryManager.inventory[pos].details;
        }
    }


    protected override void OnHover()
    {
        base.OnHover();
        GetContent();
        TooltipSystem.Show(content, header);
    }

    protected override void LeaveHover()
    {
        base.LeaveHover();
        TooltipSystem.Hide();
    }
}
