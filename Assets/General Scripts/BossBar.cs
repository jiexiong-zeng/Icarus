using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBar : MonoBehaviour
{
    Transform[] allChildren;
    HpBar health;
    EnemyCombat combat;

    // Start is called before the first frame update
    void Start()
    {
        allChildren = GetComponentsInChildren<Transform>();
        health = GetComponent<HpBar>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckBoss();
    }

    void CheckBoss()
    {
        if (GameObject.Find("Boss") != null)
        {
            combat = GameObject.Find("Boss").GetComponent<EnemyCombat>();
            foreach (Transform child in allChildren)
            {
                child.gameObject.SetActive(true);
            }
            if (health.frontFill == null)
                health.Begin((float)combat.currentHealth, (float)combat.maxHealth);
            health.Drop((float)combat.currentHealth/(float)combat.maxHealth);
        }
        else
        {
            for(int i = 1; i < allChildren.Length; i++)
            {
                allChildren[i].gameObject.SetActive(false);
            }
        }
    }

}
