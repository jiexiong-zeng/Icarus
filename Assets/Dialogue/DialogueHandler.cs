using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueHandler : MonoBehaviour
{
   
    public GameObject bubblePrefab; 
    public string[] dialogues;

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
    }

    public void HandleDialogue()
    {
        isHandling = true;
        bubble = Instantiate(bubblePrefab, transform.position + Vector3.up, transform.rotation);
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
                i = 0;
                isHandling = false;
                Destroy(bubble);
            }
        }
        
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