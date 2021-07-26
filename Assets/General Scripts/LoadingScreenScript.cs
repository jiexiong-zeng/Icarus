using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LoadingScreenScript : MonoBehaviour
{

    private GameObject SpawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        Image image = GetComponentInChildren<Image>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
        //StartCoroutine(FadeOut());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadingScreen(bool enable)
    {
        if(enable)
            StartCoroutine(FadeIn());
        else
            StartCoroutine(FadeOut());
    }


    IEnumerator FadeIn()
    {
        Image[] images = GetComponentsInChildren<Image>();
        for (float i = 0f; i <= 20; i++)
        {
            foreach (Image image in images)
                image.color = new Color(image.color.r, image.color.g, image.color.b, i / 20);
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator FadeOut() 
    {

        Image[] images = GetComponentsInChildren<Image>();
        for (float i = 50; i >= 0; i--)
        {
            foreach (Image image in images)
                image.color = new Color(image.color.r, image.color.g, image.color.b, i / 50);
            yield return new WaitForSeconds(0.01f);
        }


    }


    public void RespawnSequence()
    {
        LoadingScreen(true);
        StartCoroutine(RespawnRoutine());
    }

    IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 1;
        SpawnPoint = GameObject.Find("SpawnPoint");
        SpawnPoint.transform.position = new Vector3(PlayerPrefs.GetFloat("Respawn_x"), PlayerPrefs.GetFloat("Respawn_y"), PlayerPrefs.GetFloat("Respawn_z"));
        SceneManager.LoadScene(PlayerPrefs.GetString("RespawnScene"));
    }

}
