using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueHandler : MonoBehaviour
{
   
    public GameObject bubblePrefab; 
    public string[] dialogues;
    public Transform[] allchildren;

    GameObject bubble;
    TextMeshPro speech;
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
        //allchildren[4].gameObject.SetActive(false);
    }

    public void HandleDialogue()
    {
        isHandling = true;
        //transform.GetChild(1).gameObject.SetActive(false);
        //transform.GetChild(2).gameObject.SetActive(false);
        bubble = Instantiate(bubblePrefab, transform.position + Vector3.up, Quaternion.identity);
        speech = bubble.GetComponentInChildren<TextMeshPro>();
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
        Destroy(bubble);
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