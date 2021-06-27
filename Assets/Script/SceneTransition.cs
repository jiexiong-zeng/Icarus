using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string sceneToLoad;
    public Vector3 spawnPos;
    private GameObject SpawnPoint;
    private LoadingScreenScript loadingScreen;
    //temporary
    public GameObject TemporaryTeleportPoint;
    public Vector3 Adjustment;



    void Start()
    {
        SpawnPoint = GameObject.Find("SpawnPoint");
    }
    void Update()
    {
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            //other.transform.position = TemporaryTeleportPoint.transform.position + Adjustment;
            if (loadingScreen == null)
                loadingScreen = GameObject.Find("LoadingScreen").GetComponent<LoadingScreenScript>();
            loadingScreen.LoadingScreen(true);

            StartCoroutine(FadetoBlack());
            
        }
        //Object.DontDestroyOnLoad(GameObject.Find("HUD"));
        Object.DontDestroyOnLoad(GameObject.FindGameObjectWithTag("Player"));
    }

    IEnumerator FadetoBlack()
    {
        yield return new WaitForSeconds(0.3f);
        SpawnPoint.transform.position = spawnPos;
        SceneManager.LoadScene(sceneToLoad);
    }



}
