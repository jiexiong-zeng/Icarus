using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    public PlayerCombatScript combat;
    public Image frontFill;
    public Image backFill;
    public float timeTaken;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Begin(float current, float max)
    {
        combat = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombatScript>();
        timeTaken = 0.5f;
        frontFill = transform.Find("InnerFront").GetComponent<Image>();
        backFill = transform.Find("InnerBack").GetComponent<Image>();

        frontFill.fillAmount = current / max;
        backFill.fillAmount = current / max;
        StartCoroutine(StartBar(frontFill.fillAmount));
    }


    IEnumerator StartBar(float current)
    {
        for (float i = 0f; i <= 50; i++)
        {
            frontFill.fillAmount = Mathf.Lerp(0, current, i / 50);
            backFill.fillAmount = Mathf.Lerp(0, current, i / 50);
            yield return new WaitForSeconds(timeTaken / 50);
        } 
    }

    public void Set(float val)
    {
        frontFill.fillAmount = val;
        backFill.fillAmount = val;
    }

    public void DropHealth(float newAmount)
    {
        //Update current health
        //currentHealth = currentHealth - damage;
        //Update front immediately
        frontFill.fillAmount = newAmount;
        //Update back catches up
        backFill.color = Color.red; //Red cause damage
        //StopAllCoroutines();
        StartCoroutine(DelayHealth(backFill.fillAmount, frontFill.fillAmount, true));
    }

    public void GainHealth(float newAmount)
    {
        //Update current health
        //currentHealth = currentHealth + heals;
        //Update back immediately
        backFill.color = Color.green; //Green casue healing
        backFill.fillAmount = newAmount;
        //Update front catches up 
        //StopAllCoroutines();
        StartCoroutine(DelayHealth(frontFill.fillAmount, backFill.fillAmount, false));
    }

    IEnumerator DelayHealth(float start, float end, bool back)
    {
        for (float i = 0f; i <= 100; i++)
        {
            if (back)
                backFill.fillAmount = Mathf.Lerp(start, end, i / 100);
            else
                frontFill.fillAmount = Mathf.Lerp(start, end, i / 100);
            yield return new WaitForSeconds(timeTaken / 100);
        }
    }

    public IEnumerator Flash()
    {
        Debug.Log("Test");
        transform.Find("Outer").GetComponent<Image>().color = Color.white;
        yield return new WaitForSeconds(0.1f);
        transform.Find("Outer").GetComponent<Image>().color = Color.black;
    }


    /*
    IEnumerator regen()
    {
        yield return new WaitForSeconds(regenDelay);
        while(currentHealth < maxHealth)
        {
            currentHealth++;
            frontFill.fillAmount = currentHealth / maxHealth;
            yield return new WaitForSeconds(0.1f);
        }
    }
    */




}
