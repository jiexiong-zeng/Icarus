using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverTextScript : MonoBehaviour
{
    private InfoBox information;
    [TextArea] public string text;
    // Start is called before the first frame update
    void Start()
    {
        //GameObject info = gameObject.transform.parent.Find("Info").transform;
        //playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void OnMouseOver()
    {
        information.ShowInfo();
    }
    
    void OnPointerExit()
    {
        information.HideInfo();
    }


}
