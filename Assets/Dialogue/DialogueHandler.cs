using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueHandler : MonoBehaviour
{
   
    public GameObject bubblePrefab; 
    public string[] dialogues;
    public Transform[] allchildren;

    public GameObject bubble;
    TextMeshProUGUI speech;
    int i;
    bool inProgress;
    public bool isHandling;
    

    private void Start()
    {
        i = 0;
        inProgress = false;
        isHandling = false;
        allchildren = GetComponentsInChildren<Transform>();
        allchildren[3].gameObject.SetActive(false);
        if (bubble == null)
            bubble = GameObject.Find("Dialogue");
        //bubble.SetActive(false);
        //allchildren[4].gameObject.SetActive(false);
    }

    public void HandleDialogue()
    {
        isHandling = true;
        MenuManager.MenuOpen = true;
        //transform.GetChild(1).gameObject.SetActive(false);
        //transform.GetChild(2).gameObject.SetActive(false);

        //bubble = Instantiate(bubblePrefab, transform.position + Vector3.up, Quaternion.identity);
        //speech = bubble.GetComponentInChildren<TextMeshPro>();
        if (bubble == null)
            bubble = GameObject.Find("Dialogue");
        bubble.GetComponent<Image>().color = new Color(bubble.GetComponent<Image>().color.r, bubble.GetComponent<Image>().color.g, bubble.GetComponent<Image>().color.b, 1);
        bubble.SetActive(true);
        speech = bubble.GetComponentInChildren<TextMeshProUGUI>();
        speech.color = new Color(speech.color.r, speech.color.g, speech.color.b, 1);
        StopAllCoroutines();
        StartCoroutine(TextAnim(dialogues[i]));
    }
    
    void NextDialogue()
    {
        if(isHandling)
        {
            if (i < dialogues.Length - 1)
            {
                i++;
                StopAllCoroutines();
                StartCoroutine(TextAnim(dialogues[i]));
            }
            else
            {
                CloseDialogue();
                //transform.GetChild(1).gameObject.SetActive(true);
                //transform.GetChild(2).gameObject.SetActive(true);
                
            }
        }
        
    }

    public void CloseDialogue()
    {
        i = 0;
        isHandling = false;
        MenuManager.MenuOpen = false;
        //Destroy(bubble);
        bubble.GetComponent<Image>().color = new Color(bubble.GetComponent<Image>().color.r, bubble.GetComponent<Image>().color.g, bubble.GetComponent<Image>().color.b, 0);
        speech.color = new Color(speech.color.r, speech.color.g, speech.color.b, 0);
    }

    IEnumerator TextAnim(string Text)
    {
        inProgress = true;
        string text = "";
        foreach(char character in Text)
        {
            text = text.Insert(text.Length, character.ToString());
            speech.SetText(text);
            yield return new WaitForSeconds(.1f);
        }
        inProgress = false;
    }


    private void Update()
    {
        if (Input.GetKeyDown("return"))
        {
            if (!inProgress)
                NextDialogue();
            else
            {
                StopAllCoroutines();
                speech.SetText(dialogues[i]);
                inProgress = false;
            }
        }
    }
}