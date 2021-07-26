using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverFastTravel : Hover
{
    public ObeliskData obelisk;

    protected override void OnHover()
    {
        base.OnHover();
        GetComponent<Image>().color = new Color(255, 255, 255, 1);
        //Debug.Log(obelisk.pathToImg);
        //Debug.Log(Resources.Load<Sprite>("Obelisks\\test"));
        GameObject.Find("Screenshot").GetComponent<Image>().sprite = Resources.Load<Sprite>(obelisk.pathToImg);
        if (Input.GetMouseButtonDown(0))
        {
            gameObject.GetComponentInParent<TravelOptions>().CloseOptions(obelisk);
        }
    }

    protected override void LeaveHover()
    {
        base.LeaveHover();
        GetComponent<Image>().color = new Color(255, 255, 255, 0.25f);
    }
}
