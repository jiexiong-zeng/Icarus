using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Consumable
{
    protected override void Effect()
    {
        Debug.Log("This is for key");
        base.Effect();
    }
}
