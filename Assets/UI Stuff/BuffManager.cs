using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffManager : MonoBehaviour
{
    public static List<Buff> activeBuffs = new List<Buff>();
    public GameObject buff;
    public static BuffManager instance;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        int i = 0;
        var children = gameObject.GetComponentsInChildren<Transform>();
        for (int j = 1; j < children.Length; j++)
            Destroy(children[j].gameObject);
        //Display accordingly 
        foreach (Buff activeBuff in activeBuffs)
        {
            var prefab = Instantiate(buff, transform);
            var rect = prefab.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(i++ * 50, 0);
            prefab.GetComponentsInChildren<Image>()[0].fillAmount = ((activeBuff.time - Time.time) / activeBuff.duration);
            prefab.GetComponentsInChildren<Image>()[1].sprite = activeBuff.img;
        }
    }
}


public class Buff
{
    public Sprite img;
    public float time;
    public float duration;

    public Buff(Sprite sprite, float endTime, float dur)
    {
        img = sprite;
        time = endTime;
        duration = dur;
    }
}