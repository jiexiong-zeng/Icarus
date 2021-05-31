using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueController : MonoBehaviour
{
    public GameObject NPC;
    public GameObject player;
    public ChatBubble bubble;
    TextMeshPro speech;

    public void startDialogue(Dialogue dialogue)
    {
        //Get player
        player = GameObject.FindGameObjectWithTag("Player");
        //Get the other NPC
        NPC = GameObject.Find(dialogue.NPCname);
        //Get the chatbubble of NPC and the text
        //bubble = NPC.FindObject();
        speech = bubble.GetComponentInChildren<TextMeshPro>();

        //Run through the array and display accordingly
        int length = dialogue.dialogues.GetLength(0);
        showDialogues(dialogue, length);
        /*
        Debug.Log(length);
        for(int i = 0; i<length; i++)
        {
            Debug.Log(dialogue.dialogues[i].Text);
            if(dialogue.dialogues[i].IsPlayerSpeaking)
            {
                Debug.Log("Said by player");
            }
            else
            {
                Debug.Log("NPC");
            }
        }
        */
    }

    IEnumerator showDialogues(Dialogue dialogue, int length)
    {
        for(int i=0; i<length; i++)
        {
            speech.SetText(dialogue.dialogues[i].Text);
            yield return new WaitForSeconds(1f);
        }
    }
}
