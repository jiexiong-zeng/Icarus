using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDialogue : MonoBehaviour
{
    public float InteractionRadius;
    public DialogueController dialogueController;
    public DialogueData dialogueData;
    public LayerMask NPCLayers;

    // Update is called once per frame
    void Update()
    {
        Collider2D[] npcColliders = Physics2D.OverlapCircleAll(transform.position,InteractionRadius,NPCLayers);
        if (npcColliders.Length != 0)
        {
            string name = npcColliders[0].name;
            Debug.Log(name);
            foreach (Dialogue dialogue in dialogueData.interactions)
            {
                //Find the specific interaction
                if (dialogue.NPCname == name)
                {
                    Debug.Log("NPC name is " + dialogue.NPCname);
                    Debug.Log("First dialogue is " + dialogue.dialogues[0].Text);
                    dialogueController.startDialogue(dialogue);
                    //break;
                }
            }
        }
    }
}
