using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueController : MonoBehaviour
{
    public GameObject NPC;
    public GameObject player;
    public GameObject chatbubble;
    public TextMeshPro speech;

    public void startDialogue(Dialogue dialogue)
    {
        //Get player
        player = GameObject.FindGameObjectWithTag("Player");
        //Get the other NPC
        NPC = GameObject.Find(dialogue.NPCname);
        //Get text
        //chatbubble = GameObject.Find("Chatbubble"); //Added through editor as cant seem to find inactive components (TODO)
        //Show speechbubble
        chatbubble.SetActive(true);
        speech = chatbubble.GetComponentInChildren<TextMeshPro>();

        //Run through the array and display accordingly
        int length = dialogue.dialogues.GetLength(0);
        StartCoroutine(Delay(dialogue, length));
    }

    IEnumerator Delay(Dialogue dialogue, int length)
    {
        for (int i = 0; i < length; i++)
        {
            speech.SetText(dialogue.dialogues[i].Text);
            yield return new WaitForSeconds(1.5f);
        }
    }
}
