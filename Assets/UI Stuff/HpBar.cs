using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    public Image frontFill;
    public Image backFill;
    public float maxHealth;
    public float timeTaken;
    public float currentHealth;
    public float regenDelay;
    private float width;
    private float height;

    // Start is called before the first frame update
    void Start()
    {
        width = GetComponent<RectTransform>().sizeDelta.x;
        height = GetComponent<RectTransform>().sizeDelta.y;
        timeTaken = 0.5f;
        maxHealth = 100;
        currentHealth = maxHealth;
        frontFill = transform.Find("InnerFront").GetComponent<Image>();
        backFill = transform.Find("InnerBack").GetComponent<Image>();
        frontFill.fillAmount = 1;
        backFill.fillAmount = 1;
        regenDelay = 1;
        StartCoroutine(startBar());
    }

    IEnumerator startBar()
    {
        for (float i = 0f; i <= 200; i++)
        {
            GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Lerp(0, width, i / 200), height);
            yield return new WaitForSeconds(timeTaken / 100);
        }
    }

    public void dropHealth(float damage)
    {
        if (currentHealth > 0)
        {
            //Update current health
            currentHealth = currentHealth - damage;
            //Update front immediately
            frontFill.fillAmount = currentHealth / maxHealth;
            //Update back catches up
            backFill.color = Color.red; //Red cause damage
            StopAllCoroutines();
            StartCoroutine(delayHealth((currentHealth + damage) / maxHealth, currentHealth / maxHealth, true));
        }
        else
            currentHealth = 0;
    }

    public void gainHealth(float heals)
    {
        if (currentHealth < maxHealth)
        {
            //Update current health
            currentHealth = currentHealth + heals;
            //Update back immediately
            backFill.color = Color.green; //Green casue healing
            backFill.fillAmount = currentHealth / maxHealth;
            //Update front catches up 
            StopAllCoroutines();
            StartCoroutine(delayHealth((currentHealth - heals) / maxHealth, currentHealth / maxHealth, false));
        }
        else
            currentHealth = maxHealth;
    }

    IEnumerator delayHealth(float start, float end, bool back)
    {
        for (float i = 0f; i <= 100; i++)
        {
            Debug.Log("Hi");
            if (back)
                backFill.fillAmount = Mathf.Lerp(start, end, i / 100);
            else
                frontFill.fillAmount = Mathf.Lerp(start, end, i / 100);
            yield return new WaitForSeconds(timeTaken/100);
        }

        if (this.name == "Stamina")
            StartCoroutine(regen());
    }

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

}
