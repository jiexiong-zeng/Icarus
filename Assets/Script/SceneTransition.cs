using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string sceneToLoad;
    public Vector3 spawnPos;
    private GameObject SpawnPoint;
    void Start()
    {
        SpawnPoint = GameObject.Find("SpawnPoint");
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            SpawnPoint.transform.position = spawnPos;
            SceneManager.LoadScene(sceneToLoad);
        }
    }


}
