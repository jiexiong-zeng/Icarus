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
            string name = npcColliders[0].name;
            handler = GameObject.Find(name).GetComponent<DialogueHandler>();
            if (Input.GetKey("e"))
            {
                if (!handler.isHandling)
                {
                    handler.allchildren[3].gameObject.SetActive(false);
                    //handler.allchildren[4].gameObject.SetActive(false);
                    handler.HandleDialogue();
                }
            }

            if (!handler.isHandling && !handler.allchildren[3].gameObject.activeInHierarchy)
            {
                handler.allchildren[3].gameObject.SetActive(true);
                //handler.allchildren[4].gameObject.SetActive(true);
            }

        }

        else
        {
            if (handler != null)
            {
                //Debug.Log(handler.gameObject.name);
                handler.allchildren[3].gameObject.SetActive(false);
                handler.CloseDialogue();
                handler = null;
                Debug.Log(handler);
                //handler.allchildren[4].gameObject.SetActive(false);
            }
        }
            

    }
}
