using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueHandler : MonoBehaviour
{
   public GameObject bubble;
   public TextMeshPro speech;
   public DialogueData dialogueData;
   int i;
   Dialogue test;

    private void Start()
    {
        i = 0;
        //Get Dialogue data
        dialogueData = GameObject.Find("DialogueData").GetComponent<DialogueData>();
        //Get speechbubble(inspector)
        //Get speech 
        speech = bubble.GetComponentInChildren<TextMeshPro>();
    }

    public void HandleDialogue()
    {
        //Get dialogue
        foreach (Dialogue dialogue in dialogueData.interactions)
        {
            //Find the specific interaction
            if (dialogue.NPCname == name)
            {
                test = dialogue;
                //Activate the bubble
                bubble.SetActive(true);
                //Set text
                StopAllCoroutines();
                StartCoroutine(TextAnim(dialogue.dialogues[i].Text));
            }
        }
    }
    
    void NextDialogue()
    {
        if (i < test.dialogues.Length - 1 && bubble.activeSelf)
        {
            i++;
            StopAllCoroutines();
            StartCoroutine(TextAnim(test.dialogues[i].Text));
        }
        else
        {
            i = 0;
            bubble.SetActive(false);
        }
    }

    /*
    IEnumerator ShowDialogues(Dialogue dialogue)
    {
        int length = dialogue.dialogues.GetLength(0);
        for (int i = 0; i < length; i++)
        {
            //To create a typing effect
            StartCoroutine(TextAnim(dialogue.dialogues[i].Text));
            yield return new WaitForSeconds(.15f * dialogue.dialogues[i].Text.Length);
        }
    }
    */

    IEnumerator TextAnim(string Text)
    {
        string text = "";
        foreach(char character in Text)
        {
            text = text.Insert(text.Length, character.ToString());
            speech.SetText(text);
            yield return new WaitForSeconds(.1f);
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown("return"))
        {
            Debug.Log("yo");
            NextDialogue();
        }
    }
}