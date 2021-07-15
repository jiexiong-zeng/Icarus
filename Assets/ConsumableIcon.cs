using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConsumableIcon : MonoBehaviour
{
    Image img;
    TextMeshProUGUI num;
    
    // Start is called before the first frame update
    void Start()
    {
        img = GetComponentsInChildren<Image>()[1];
        num = GetComponentInChildren<TextMeshProUGUI>();
        img.enabled = false;
        num.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (InventoryManager.inventory.Count != 0)
        {
            img.enabled = true;
            num.enabled = true;
            img.sprite = InventoryManager.inventory[InventoryManager.activeSlot].sprite;
            num.SetText(InventoryManager.inventory[InventoryManager.activeSlot].amount.ToString());
            if (Input.mouseScrollDelta != Vector2.zero)
            {
                //Debug.Log("Increasing");
                InventoryManager.activeSlot = (InventoryManager.activeSlot + (int)Input.mouseScrollDelta.y);
                if (InventoryManager.activeSlot < 0)
                    InventoryManager.activeSlot = InventoryManager.inventory.Count - 1;
                InventoryManager.activeSlot = InventoryManager.activeSlot % InventoryManager.inventory.Count;
            }
        }

        else
        {
            img.enabled = false;
            num.enabled = false;
        }

        if(Input.GetKeyDown("q"))
        {
            InventoryManager.inventory[InventoryManager.activeSlot].Consume();
        }
    }
}
