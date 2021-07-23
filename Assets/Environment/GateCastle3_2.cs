using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateCastle3_2 : leverScript
{ 
    public static bool isOpen;
    public static Vector3 stopPos;

    // Start is called before the first frame update
    void Start()
    {
        door = GetComponent<leverScript>().door;
    }

    // Update is called once per frame
    void Update()
    {
        isOpen = open | isOpen;
        if (open)
            stopPos = door.transform.position;
        if (isOpen)
            door.transform.position = stopPos;
    }
}
