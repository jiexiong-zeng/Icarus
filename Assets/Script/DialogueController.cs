using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    public GameObject NPC;
    public GameObject player;

    public void startDialogue(Dialogue dialogue)
    {
        //Get player
        player = GameObject.FindGameObjectWithTag("Player");
        //Get the other NPC
        NPC = GameObject.Find(dialogue.NPCname);

        //Run through the array and display accordingly
        int length = dialogue.dialogues.GetLength(0);
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
    }
}
