using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnPointScript : MonoBehaviour
{
    public int SpawnPointNumber = 0;
    private bool active = false;


    private bool saved = false;
    private bool unique = true;
    private bool travelling = false;
    GameObject spawnPoint;
    TravelOptions options;
    
    //public int distance = 100;
    // Start is called before the first frame update
    void Start()
    {
        options = GameObject.Find("Fast Travel").GetComponent<TravelOptions>();
        spawnPoint = GameObject.Find("SpawnPoint");
        SaveLoad.LoadObelisks();
        if (PlayerPrefs.GetInt("SpawnPointNumber") == SpawnPointNumber)
        {
            active = true;
            GetComponent<Animator>().Play("Obelisk2");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (PlayerPrefs.GetInt("SpawnPointNumber") != SpawnPointNumber)
        {
            active = false;
            GetComponent<Animator>().Play("Obelisk3");
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetButton("Interact"))
        {
            if (active == false)
            {
                active = true;
                GetComponent<Animator>().Play("Obelisk2");
                PlayerPrefs.SetInt("SpawnPointNumber", SpawnPointNumber);
            }
            PlayerPrefs.SetString("RespawnScene", SceneManager.GetActiveScene().name);
            PlayerPrefs.SetFloat("Respawn_x", transform.position.x);
            PlayerPrefs.SetFloat("Respawn_y", transform.position.y);
            PlayerPrefs.SetFloat("Respawn_z", transform.position.z);

            if(!saved)
            {
                var thisObelisk = new ObeliskData(SceneManager.GetActiveScene().name, transform.position, SpawnPointNumber);
                foreach (var obelisk in SaveLoad.savedObelisks)
                {
                    if (thisObelisk.num == obelisk.num)
                        unique = false;
                }
                if(unique)
                    SaveLoad.SaveObelisk(thisObelisk);
                saved = !saved;
            }

            GameObject spawnPoint = GameObject.Find("SpawnPoint");
            spawnPoint.transform.position = transform.position;
        }

        if (Input.GetKey("return"))
        {
            if (!true)
            {
                Debug.Log("Fast travel");
                StartCoroutine(FastTravel(SaveLoad.savedObelisks[0]));
                travelling = true;
            }

            options.ShowOptions(this);
        }
    }

    IEnumerator FastTravel(ObeliskData tp)
    {
        var loadingScreen = GameObject.Find("LoadingScreen").GetComponent<LoadingScreenScript>();
        loadingScreen.LoadingScreen(true);
        yield return new WaitForSeconds(0.3f);
        spawnPoint.transform.position = new Vector3(tp.position[0], tp.position[1], tp.position[2]);
        var areaName = GameObject.Find("Area Name").GetComponent<FadeIn>();
        areaName.ShowText(tp.scene);
        SceneManager.LoadScene(tp.scene);
    }

    public void FastTravelRoutine(ObeliskData tp)
    {
        StartCoroutine(FastTravel(tp));
    }

}
