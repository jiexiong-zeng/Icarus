using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class SkillWheel : MonoBehaviour
{
    Transform[] allChildren;
    GameObject player;
    RectTransform pos;
    int numSectors;
    int sectorComps;
    public static int selected;

    // Start is called before the first frame update
    void Start()
    {
        pos = GetComponent<RectTransform>();
        allChildren = GetComponentsInChildren<Transform>();
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(Smoothen());
        sectorComps = allChildren[1].GetComponentsInChildren<Transform>().Length;
        numSectors = (allChildren.Length - 1) / sectorComps;
        //Debug.Log(sectorComps);
        //Debug.Log(numSectors);
        for (int i = 1; i < allChildren.Length; i+=sectorComps)
            allChildren[i].gameObject.SetActive(false);
        //Lock(1);
        //Lock(2);
        //Lock(3);
        //Lock(4);
    }

    // Update is called once per frame
    void Update()
    {
        var brain = Camera.main.GetComponent<CinemachineBrain>();
        if (brain.m_BlendUpdateMethod != CinemachineBrain.BrainUpdateMethod.LateUpdate)
            brain.m_BlendUpdateMethod = CinemachineBrain.BrainUpdateMethod.LateUpdate;

        if (player == null)
        {
            //StopAllCoroutines();
            player = GameObject.FindGameObjectWithTag("Player");
        }
        if(player != null)
        {
            //StartCoroutine(Smoothen());
            Vector3 playerToMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.transform.position;
            float angle = Vector2.SignedAngle(Vector2.right, playerToMouse);
            int selectedSector = (angle > 0) ? Mathf.FloorToInt(angle * numSectors / 360) : Mathf.FloorToInt((360 + angle) * numSectors / 360);
            //var targetpos = Camera.main.WorldToScreenPoint(player.transform.position);
            //transform.position = targetpos;
            //Debug.Log(selectedSector);
            if (Input.GetKey("tab"))
            {
                for (int i = 1; i < allChildren.Length; i += sectorComps)
                {
                    allChildren[i].gameObject.SetActive(true);
                    var bg = allChildren[i + 1].gameObject;
                    if (allChildren[i + 2].gameObject.GetComponent<Skill>().isUnlocked)
                        bg.GetComponent<Image>().color = (i == 1 + selectedSector * sectorComps) ? new Color(255, 255, 255, 1) : new Color(255, 255, 255, 0.25f);
                    else
                        bg.GetComponent<Image>().color = Color.black;
                }

                if (allChildren[3 + sectorComps * selectedSector].gameObject.GetComponent<Skill>().isUnlocked)
                    selected = selectedSector;
            }
            else
                for (int i = 1; i < allChildren.Length; i += sectorComps)
                    allChildren[i].gameObject.SetActive(false);
        }
        //Debug.Log(selected);
    }

    IEnumerator Smoothen()
    {
        while (true)
        {
            pos.position = RectTransformUtility.WorldToScreenPoint(Camera.main, player.transform.position);
            yield return new WaitForSecondsRealtime(Time.deltaTime * 10);
        }
    }

    public void Lock(int sector)
    {
        allChildren[3 + sectorComps * sector].gameObject.GetComponent<Skill>().isUnlocked = false;
    }

    public void Unlock(int sector)
    {
        allChildren[3 + sectorComps * sector].gameObject.GetComponent<Skill>().isUnlocked = true;
    }

}
