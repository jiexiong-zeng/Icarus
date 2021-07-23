using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Invis : Consumable
{
    public SpriteRenderer image;
    public PlayerMovement_MysteryKnight move;
    public Sprite sprite;
    public float duration = 10;

    protected override void Effect()
    {
        base.Effect();
        image = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();
        move = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement_MysteryKnight>();
        sprite = GetComponent<SpriteRenderer>().sprite;
        StartCoroutine(EffectDur());
    }

    IEnumerator EffectDur()
    {
        float endTime = Time.time + duration;
        for (float i = 0f; i <= 50; i++)
        { 
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1 - (i / 100) );
            yield return new WaitForSeconds(0.01f);
        }
        //var enemies = GetGameObjectsInLayer(LayerMask.NameToLayer("Enemy"));
        var thisBuff = new Buff(sprite, endTime, duration);
        BuffManager.activeBuffs.Add(thisBuff);
        //Debug.Log(BuffManager.activeBuffs.Contains(thisBuff));
        
        while (Time.time < endTime)
        {
            GameObject.FindGameObjectWithTag("Player").layer = LayerMask.NameToLayer("Invisible");
            yield return null;


            /*
            //Get all enemy objects
            //Get controller
            foreach(GameObject enemy in enemies)
            {
                Component[] components = enemy.GetComponents<Component>();
                foreach (Component component in components)
                {
                    //Debug.Log(component.GetType().ToString());
                    if(component.GetType().ToString().Contains("Controller") && component.GetType().ToString() != "EnemyController")
                    {
                        Debug.Log(component.GetType().ToString());
                        var controller = GetComponent(component.GetType());
                        controller.aggroed = false;
                    }
                }
                //Debug.Log(enemy.name);
            }
            //Set aggroed to false
            */
        }
        GameObject.FindGameObjectWithTag("Player").layer = LayerMask.NameToLayer("Player");
        BuffManager.activeBuffs.Remove(thisBuff);

        for (float i = 0f; i <= 50; i++)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0.5f + (i / 50));
            yield return new WaitForSeconds(0.01f);
        }
    }

    GameObject[] GetGameObjectsInLayer(int layer)
    {
        var allArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        var reqList = new List<GameObject>();
        foreach (GameObject go in allArray)
            if (go.layer == layer)
                reqList.Add(go);
        if (reqList.Count == 0)
        {
            return null;
        }
        return reqList.ToArray();
    }
}
