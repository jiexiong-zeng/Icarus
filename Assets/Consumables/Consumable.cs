using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : MonoBehaviour
{
    [TextArea(15, 20)]
    public string content;
    protected InventoryManager inventoryManager;
    Vector3 startPos;

    // Start is called before the first frame update
    protected void Start()
    {
        startPos = transform.position;
        inventoryManager = GameObject.Find("Inventory").GetComponent<InventoryManager>();
        foreach (Vector3 position in InventoryManager.collected)
        {
            if (transform.position == position)
                Destroy(gameObject);
        }
    }

    protected virtual void Effect()
    {
        //Debug.Log("Consumed");
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
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            //Debug.Log("Added to consumables");
            inventoryManager.AddToGrid(gameObject, Effect, content);
            InventoryManager.collected.Add(startPos);
            //Debug.Log(InventoryManager.collected.Count);
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        }
    }
}
