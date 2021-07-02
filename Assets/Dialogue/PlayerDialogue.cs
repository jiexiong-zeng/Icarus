using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDialogue : MonoBehaviour
{
    public float InteractionRadius;
    public LayerMask NPCLayers;
    //public GameObject Prompt;

    DialogueHandler handler;

    // Update is called once per frame
    void Update()
    {
        Collider2D[] npcColliders = Physics2D.OverlapCircleAll(transform.position,InteractionRadius,NPCLayers);
        if (npcColliders.Length != 0)
        {

            if (Input.GetKey("e"))
            {
                string name = npcColliders[0].name;
                handler = GameObject.Find(name).GetComponent<DialogueHandler>();
                if(!handler.isHandling)
                    handler.HandleDialogue();
            }

        }

    }
}
