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
        mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        vcam = gameObject.GetComponent<CinemachineVirtualCamera>();
        vcam.Follow = transform.parent;
        CinemachineBrain brain = mainCam.GetComponent<CinemachineBrain>();
        yield return new WaitForSeconds(delay);
        vcam.Priority = 11;
        yield return new WaitForSeconds(0.02f);
        while (brain.IsBlending)
            yield return null;
        yield return new WaitForSeconds(delay);
        vcam.Priority = 1;
    }

}
