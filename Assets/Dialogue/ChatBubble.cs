using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatBubble : MonoBehaviour
{
    private TextMeshPro textMesh;

    // Start is called before the first frame update
    void Start()
    {
        //Make it show up properly
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        textMesh = transform.Find("Text").GetComponent<TextMeshPro>();
        textMesh.SetText("");
        textMesh.ForceMeshUpdate();
        //Hide until required
        this.gameObject.SetActive(false);
    }

}
