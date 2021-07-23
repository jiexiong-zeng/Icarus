using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static List<InventoryObject> inventory = new List<InventoryObject>();
    public int slotsFilled = 0;
    public static int activeSlot = -9999;
    //public static InventoryObject activeItem = null;

    public GameObject grid;
    public Transform[] children;
    int numComponents;

    // Start is called before the first frame update
    void Start()
    {
        slotsFilled = 0;
        //Get grid and children of grid
        children = grid.GetComponentsInChildren<Transform>();
        numComponents = children[1].GetComponentsInChildren<Transform>().Length;
        for (int i = 1; i < children.Length; i++)
            children[i].gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public delegate void MyDelegateType();

    public void AddToGrid(GameObject item, MyDelegateType inputFunction)
    {
        if (inventory.Find(x => x.name == item.name.Split(' ')[0]) != null)
        {
            Debug.Log("Incrementing");
            var inventoryItem = inventory.Find(x => x.name == item.name.Split(' ')[0]);
            inventoryItem.amount++;
            //if (activeItem.name == inventoryItem.name)
                //activeItem.amount = inventoryItem.amount;
            children[3 + inventoryItem.slot * numComponents].GetComponent<TextMeshProUGUI>().SetText(inventoryItem.amount.ToString());
        }

        else
        {
            //Make active if nothing else exists
            if (slotsFilled == 0)
            {
                activeSlot = 0;
                //activeItem = new InventoryObject(item.name, slotsFilled, item.GetComponent<SpriteRenderer>().sprite);
                //Debug.Log(activeItem.name);
            }
            //Add to collected
            inventory.Add(new InventoryObject(item.name.Split(' ')[0], slotsFilled, item.GetComponent<SpriteRenderer>().sprite, () => inputFunction() ));
            //Add to inventory
            for (int i = 0; i < numComponents; i++)
                children[1 + slotsFilled* numComponents + i].gameObject.SetActive(true);
            children[2 + slotsFilled*numComponents].GetComponent<Image>().sprite = item.GetComponent<SpriteRenderer>().sprite;
            slotsFilled++;
        }

        //inputFunction();
    }

}

[System.Serializable]
public class InventoryObject
{
    public int amount;
    public int slot;
    public string name;
    public Sprite sprite;
    FunctionDelegate OnConsume;

    public delegate void FunctionDelegate();

    //public void Consume();

    public InventoryObject(string nameInput, int num, Sprite img, FunctionDelegate function)
    {
        name = nameInput;
        amount = 1;
        slot = num;
        sprite = img;
        OnConsume = function;
    }

    public void Consume()
    {
        OnConsume();
    }

}