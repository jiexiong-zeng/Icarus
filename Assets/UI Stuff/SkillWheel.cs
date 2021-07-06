using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillWheel : MonoBehaviour
{
    Transform[] allChildren;
    GameObject player;
    RectTransform pos;

    // Start is called before the first frame update
    void Start()
    {
        pos = GetComponent<RectTransform>();
        allChildren = GetComponentsInChildren<Transform>();
        player = GameObject.FindGameObjectWithTag("Player");
        for (int i = 1; i < allChildren.Length; i++)
            allChildren[i].gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        pos.position = RectTransformUtility.WorldToScreenPoint(Camera.main, player.transform.position);
        if (Input.GetKeyDown("tab"))
            for (int i = 1; i < allChildren.Length; i++)
                allChildren[i].gameObject.SetActive(!allChildren[i].gameObject.activeSelf);
    }
}
