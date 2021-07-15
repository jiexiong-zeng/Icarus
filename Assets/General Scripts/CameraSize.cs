using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSize : MonoBehaviour
{
    // Start is called before the first frame update
    private CinemachineVirtualCamera vcam;
    public float size = 2.5f;
    public float transitionDuration = 2f;
    private float targetSize, initialSize, currentSize;
    //private initialSize;

    void Start()
    {
        var camera = Camera.main;
        var brain = (camera == null) ? null : camera.GetComponent<CinemachineBrain>();
        vcam = brain.ActiveVirtualCamera as CinemachineVirtualCamera;
        initialSize = vcam.m_Lens.OrthographicSize;
    }

    void FixedUpdate()
    {
        currentSize = vcam.m_Lens.OrthographicSize;

        if (currentSize < targetSize)
        {
            Debug.Log("er1");
            vcam.m_Lens.OrthographicSize += 0.01f;
        }

        else if (currentSize > targetSize)
        {
            Debug.Log("er2");
            vcam.m_Lens.OrthographicSize -= 0.01f;
        }


    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            targetSize = size;
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            targetSize = initialSize;
        }
    }

}