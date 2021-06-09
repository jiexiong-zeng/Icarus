using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharSelect : MonoBehaviour
{

    [SerializeField] Sprite[] sprites;
    public int pos;
    private void Start()
    {
        pos = 0;        
    }

    private void OnEnable()
    {
        OpenMenu();    
    }

    public void OpenMenu()
    {
        //Get first/current char and display
        transform.Find("Sprite").GetComponent<Image>().sprite = sprites[pos];
        //Set timescale to zero
        Time.timeScale = 0;
    }

    public void closeMenu()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("right"))
        {
            Debug.Log("Next char");
            transform.Find("Sprite").GetComponent<Image>().sprite = sprites[++pos % sprites.Length];
        }
        else if (Input.GetKeyDown("left"))
        {
            Debug.Log("Prev char");
            transform.Find("Sprite").GetComponent<Image>().sprite = sprites[--pos % sprites.Length];
        }
        else if (Input.GetKeyDown("escape"))
            closeMenu();
    }
}
