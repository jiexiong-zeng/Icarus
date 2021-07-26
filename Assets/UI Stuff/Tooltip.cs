using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI headerField;
    public TextMeshProUGUI contentField;

    public LayoutElement layoutElement;
    public int characterWrapLimit;

    public void SetText(string content, string header = "")
    {
        //Debug.Log(TooltipSystem.current.tooltip);
        //Debug.Log(headerField);
        //Debug.Log(TooltipSystem.current.tooltip.headerField);
        if (string.IsNullOrEmpty(header))
        {
            headerField.gameObject.SetActive(false);
        }
        else
        {
            headerField.gameObject.SetActive(true);
            headerField.text = header;
        }
        contentField.text = content;  

        int headerLength = headerField.text.Length;
        int contentLength = contentField.text.Length;

        layoutElement.enabled = (headerLength > characterWrapLimit || contentLength > characterWrapLimit) ? true : false;


    }

    // Update is called once per frame
    void Update()
    {
        Vector2 position = Input.mousePosition;
        if (Input.mousePosition.x < 960)
            transform.position = position;
        else
            transform.position = new Vector3(position.x - gameObject.GetComponent<RectTransform>().sizeDelta.x * 1.7f, position.y, 0f);
    }
}
