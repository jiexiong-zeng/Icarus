using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : MonoBehaviour
{
    protected InventoryManager inventoryManager;

    // Start is called before the first frame update
    protected void Start()
    {
        inventoryManager = GameObject.Find("Inventory").GetComponent<InventoryManager>();
    }

    protected virtual void Effect()
    {
        Debug.Log("Consumed");
        InventoryManager.inventory[InventoryManager.activeSlot].amount--;
        if (InventoryManager.inventory[InventoryManager.activeSlot].amount == 0)
            InventoryManager.inventory.RemoveAt(InventoryManager.activeSlot);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.gameObject.tag == "Player")
        {
            Debug.Log("Added to consumables");
            inventoryManager.AddToGrid(this.gameObject, Effect);
            Destroy(gameObject);
        }
    }
}
