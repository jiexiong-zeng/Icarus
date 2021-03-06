using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TravelOptions : MonoBehaviour
{
    public GameObject buttonPrefab;
    SpawnPointScript reference;
    public int numChild;
    public bool isShowing;
    //public static ObeliskData selected;
    Transform[] children;

    // Start is called before the first frame update
    void Start()
    {
        //selected = null;
        children = gameObject.GetComponentsInChildren<Transform>();
        isShowing = false;
        for (int i = 1; i < children.Length; i++)
            children[i].gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown("o"))
            //ShowOptions();
        //if (Input.GetKeyUp("o"))
            //Debug.Log("Released");
    }


    //Fast-Travel
    //  -> BG
    //     -> MenuItems 
    //          -> Panel
    //     -> Screenshot

    public void ShowOptions(SpawnPointScript instance)
    {
        MenuManager.MenuOpen = true;
        reference = instance;
        StopAllCoroutines();
        StartCoroutine(changeState());
        for (int j = 1; j < children.Length; j++)
            children[j].gameObject.SetActive(true);
        //Debug.Log(SaveLoad.savedObelisks.Count);
        //Debug.Log(gameObject.GetComponentsInChildren<Transform>().Length);
        if(gameObject.GetComponentsInChildren<Transform>().Length == numChild)
            foreach(ObeliskData obelisk in SaveLoad.savedObelisks)
            {
                //Debug.Log(transform.GetChild(0).name);
                //Debug.Log(transform.GetChild(0).GetChild(0).name);
                var prefab = Instantiate(buttonPrefab, transform.GetChild(0).GetChild(0).GetChild(0));
                //var rect = prefab.GetComponent<RectTransform>();
                //rect.anchoredPosition = new Vector2(0, ++i * -40);
                prefab.GetComponentInChildren<TextMeshProUGUI>().SetText(obelisk.scene);
                var button = prefab.GetComponent<HoverFastTravel>();
                button.obelisk = obelisk;
            }
    }


    public void CloseOptions(ObeliskData obelisk = null)
    {
        MenuManager.MenuOpen = false;
        StopAllCoroutines();
        StartCoroutine(changeState());
        //delete all
        Transform[] allChildren = transform.GetChild(0).gameObject.GetComponentsInChildren<Transform>();
        for (int i = 1; i < allChildren.Length; i ++)
            if(allChildren[i].name != "Screenshot" && allChildren[i].name != "MenuItems" && allChildren[i].name != "Panel")
                Destroy(allChildren[i].gameObject);
        for (int i = 1; i < children.Length; i++)
            children[i].gameObject.SetActive(false);
        //Fast travel
        if (obelisk != null)
            reference.FastTravelRoutine(obelisk);

    }

    IEnumerator changeState()
    { 
        yield return new WaitForSeconds(0.5f);
        //Debug.Log("Swapping");
        isShowing = !isShowing;
    }

}
