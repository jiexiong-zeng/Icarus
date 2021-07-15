using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using TMPro;

public class SceneManagerScript : MonoBehaviour
{
    private CinemachineVirtualCamera followCam;
    private LoadingScreenScript loadingScreen;
    public GameObject PlayerPrefab;

    public static SceneManagerScript instance = null;

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    // called first
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        if (Player == null)
            Player = Instantiate(PlayerPrefab, transform.position, Quaternion.identity);
        Player.transform.position = transform.position;
        
        followCam = GameObject.Find("Cinemachine").GetComponent<CinemachineVirtualCamera>();
        followCam.Follow = Player.transform;
        followCam.m_Lens.OrthographicSize = 2;
        
        if (loadingScreen == null)
            loadingScreen = GameObject.Find("LoadingScreen").GetComponent<LoadingScreenScript>();
        loadingScreen.LoadingScreen(false);

        GameObject areaName = GameObject.Find("Area Name");
        areaName.GetComponent<TextMeshProUGUI>().SetText(scene.name);
        if(scene.name == "Forest 1")
        {
            PlayerPrefs.SetInt("SpawnPointNumber",-1);
        }
    }

    // called when the game is terminated
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
