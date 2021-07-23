using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Resume()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
        MenuManager.MenuOpen = false;
    }

    public void Exit()
    {
        Time.timeScale = 1;
        Application.Quit();
    }


}
