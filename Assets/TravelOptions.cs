using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TravelOptions : MonoBehaviour
{
    public GameObject buttonPrefab;
    SpawnPointScript reference;
    //public static ObeliskData selected;

    // Start is called before the first frame update
    void Start()
    {
        //selected = null;
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
        int i = 0;
        //Debug.Log(SaveLoad.savedObelisks.Count);
        if(gameObject.GetComponentsInChildren<Transform>().Length < 2)
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


    public void CloseOptions(ObeliskData obelisk)
    {
        //delete all
        Transform[] allChildren = GetComponentsInChildren<Transform>();
        for (int i = 1; i < allChildren.Length; i ++)
            Destroy(allChildren[i].gameObject);
        //Fast travel
        reference.FastTravelRoutine(obelisk);

    }

}
