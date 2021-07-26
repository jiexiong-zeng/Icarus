using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;


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
        //Debug.Log(Input.GetAxisRaw("Horizontal"));
        selectedy = transform.GetChild((int)selected).position.y;
        arrow.position = new Vector2(arrow.position.x, selectedy);
        selected = selected + Input.GetAxisRaw("Vertical");
        if (selected > transform.childCount - 1)
            selected = 1;
        else if (selected < 1)
            selected = transform.childCount - 1;
    }

    public void LoadGame()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
        Debug.Log(PlayerPrefs.GetString("RespawnScene", "No Scene"));
        if (PlayerPrefs.GetString("RespawnScene", "No Scene") != "No Scene")
        {
            SceneManager.LoadScene(PlayerPrefs.GetString("RespawnScene"));
            var SpawnPoint = GameObject.Find("SpawnPoint");
            SpawnPoint.transform.position = new Vector3(PlayerPrefs.GetFloat("Respawn_x"), PlayerPrefs.GetFloat("Respawn_y"), PlayerPrefs.GetFloat("Respawn_z"));
        }

        else
        {
            SceneManager.LoadScene(PlayerPrefs.GetString("Sanctuary"));
            var obelisk = GameObject.Find("Obelisk");
            SaveLoad.SaveObelisk(new ObeliskData("Sanctuary", obelisk.transform.position, 0));
        }
    }

    public void NewGame()
    {
        PlayerPrefs.DeleteAll();
        File.Delete(Application.persistentDataPath + "/travelpoints.info");
        Debug.Log(File.Exists(Application.persistentDataPath + "/travelpoints.info"));
        LoadGame();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
