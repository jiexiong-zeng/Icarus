using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class CharSelect : MonoBehaviour
{

    public Character[] chars;
    public int pos;
    private void Start()
    {
        pos = 0;        
    }

    private void OnEnable()
    {
        OpenMenu();    
    }

    void OpenMenu()
    {
        Debug.Log(transform.Find("Sprite"));
        //Get first/current char and display
        transform.Find("Sprite").GetComponent<Image>().sprite = chars[pos].sprite;
        //Set timescale to zero
        Time.timeScale = 0;
    }

    void closeMenu()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    void selectChar()
    {
        GameObject currChar = GameObject.FindGameObjectWithTag("Player");
        Debug.Log(currChar.name);
        //Get new char
        GameObject newChar = Instantiate(chars[pos].prefab, currChar.transform.position, currChar.transform.rotation);
        //Set camera to char
        GameObject camera = GameObject.Find("CM vcam1");
        camera.GetComponent<CinemachineVirtualCamera>().Follow = newChar.transform;
        //Remove prev char
        Destroy(currChar);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("right"))
        {
            pos = ++pos % chars.Length;
            Debug.Log("Next char");
            transform.Find("Sprite").GetComponent<Image>().sprite = chars[pos].sprite;
            transform.Find("Name").GetComponent<TextMeshProUGUI>().SetText(chars[pos].name);
        }
        /*
        else if (Input.GetKeyDown("left"))
        {
            Debug.Log("Prev  char");
            transform.Find("Sprite").GetComponent<Image>().sprite = chars[--pos % chars.Length].sprite;
            transform.Find("Name").GetComponent<TextMeshProUGUI>().SetText(chars[--pos % chars.Length].name);
        }
        */
        else if (Input.GetKeyDown("escape"))
            closeMenu();
        else if (Input.GetKeyDown("return"))
        {
            selectChar();
            closeMenu();
        }
    }
}




[System.Serializable]
public class Character
{
   public Sprite sprite;
   public string name;
   public GameObject prefab;
}