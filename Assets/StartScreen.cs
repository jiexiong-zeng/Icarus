using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreen : MonoBehaviour
{
    public float selected = 1;
    Transform arrow;
    float selectedy;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
        selected = 1;
        arrow = transform.GetChild(transform.childCount - 1);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Input.GetAxisRaw("Horizontal"));
        selectedy = transform.GetChild((int)selected).position.y;
        arrow.position = new Vector2(arrow.position.x, selectedy);
        selected = selected + Input.GetAxisRaw("Horizontal");
        if (selected > transform.childCount - 1)
            selected = 1;
        else if (selected < 1)
            selected = transform.childCount - 1;
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
