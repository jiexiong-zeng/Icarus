using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Pan : MonoBehaviour
{
    public float delay;
    public bool proximity;
    public float range;
    //public GameObject followObj;


    GameObject mainCam;
    CinemachineVirtualCamera vcam;
    Transform player;

    bool hasPanned;

    // Start is called before the first frame update
    void Start()
    {
        hasPanned = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (proximity && Mathf.Abs(Vector3.Distance(player.position, transform.parent.position)) < range && !hasPanned)
            PanToObject();       
    }

    public void PanToObject()
    {
        StartCoroutine(PanRoutine());
        hasPanned = true;
    }

    IEnumerator PanRoutine()
    {
        Debug.Log("Za worldo");
        mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        vcam = gameObject.GetComponent<CinemachineVirtualCamera>();
        vcam.Follow = transform.parent;
        CinemachineBrain brain = mainCam.GetComponent<CinemachineBrain>();
        brain.m_BlendUpdateMethod = CinemachineBrain.BrainUpdateMethod.LateUpdate;
        brain.m_IgnoreTimeScale = true;
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 0;
        vcam.Priority = 11;
        yield return new WaitForSecondsRealtime(0.02f);
        while (brain.IsBlending)
            yield return null;
        yield return new WaitForSecondsRealtime(delay);
        vcam.Priority = 1;
        yield return new WaitForSecondsRealtime(0.02f);
        while (brain.IsBlending)
            yield return null;
        Time.timeScale = 1;
    }

}
