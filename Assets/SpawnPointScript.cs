using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnPointScript : MonoBehaviour
{
    public int SpawnPointNumber = 0;
    private bool active = false;
    
    //public int distance = 100;
    // Start is called before the first frame update
    void Start()
    {
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

            GameObject spawnPoint = GameObject.Find("SpawnPoint");
            spawnPoint.transform.position = transform.position;
        }

    }
}
