using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TravelOptions : MonoBehaviour
{
    public GameObject buttonPrefab;
    SpawnPointScript reference;
    public bool isShowing;
    //public static ObeliskData selected;

    // Start is called before the first frame update
    void Start()
    {
        //selected = null;
        isShowing = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown("o"))
            //ShowOptions();
        //if (Input.GetKeyUp("o"))
            //Debug.Log("Released");
    }


    public void ShowOptions(SpawnPointScript instance)
    {
        reference = instance;
        StopAllCoroutines();
        StartCoroutine(changeState());
        int i = 0;
        //Debug.Log(SaveLoad.savedObelisks.Count);
        if(gameObject.GetComponentsInChildren<Transform>().Length < 3)
            foreach(ObeliskData obelisk in SaveLoad.savedObelisks)
            {
                var prefab = Instantiate(buttonPrefab, transform);
                var rect = prefab.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(0, ++i * -40);
                prefab.GetComponentInChildren<TextMeshProUGUI>().SetText(obelisk.scene);
                var button = prefab.GetComponent<Hover>();
                button.obelisk = obelisk;
            }
    }


    public void CloseOptions(ObeliskData obelisk = null)
    {
        StopAllCoroutines();
        StartCoroutine(changeState());
        //delete all
        Transform[] allChildren = GetComponentsInChildren<Transform>();
        for (int i = 1; i < allChildren.Length; i ++)
            if(allChildren[i].name != "Screenshot")
                Destroy(allChildren[i].gameObject);
        //Fast travel
        if(obelisk != null)
            reference.FastTravelRoutine(obelisk);

    }

    IEnumerator changeState()
    { 
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Swapping");
        isShowing = !isShowing;
    }

}
