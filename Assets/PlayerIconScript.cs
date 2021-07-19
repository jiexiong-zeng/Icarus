using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIconScript : MonoBehaviour
{
    public Image background;
    public GameObject SamuraiIcon;
    public GameObject ThunderIcon;
    public GameObject FireIcon;
    public GameObject LifeIcon;
    public GameObject WaterIcon;
    private static GameObject currentIcon;

    public GameObject SamuraiSkillDescription;
    public GameObject ThunderSkillDescription;
    public GameObject FireSkillDescription;
    public GameObject LifeSkillDescription;
    public GameObject WaterSkillDescription;
    private static GameObject currentDescription;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (SkillWheel.selected)
        {
            case 0: 
                background.color = new Color32(219, 219, 219, 220);
                ChangeIcon(SamuraiIcon);
                ChangeDescription(SamuraiSkillDescription);
                break;
            case 1: 
                background.color = new Color32(255, 229, 82, 255);
                ChangeIcon(ThunderIcon);
                ChangeDescription(ThunderSkillDescription);
                break;
            case 2: 
                background.color = new Color32(219, 76, 61, 220);
                ChangeIcon(FireIcon);
                ChangeDescription(FireSkillDescription);
                break;
            case 3:
                background.color = new Color32(39, 219, 123, 220);
                ChangeIcon(LifeIcon);
                ChangeDescription(LifeSkillDescription);
                break;
            case 4:
                background.color = new Color32(48, 219, 219, 220);
                //background.color = new Color32(65, 50, 219, 220);
                ChangeIcon(WaterIcon);
                ChangeDescription(WaterSkillDescription);
                break;
        }

    }


    private void ChangeIcon(GameObject target)
    {
        if(currentIcon != target)
        {
            target.SetActive(true);
            if(currentIcon)
                currentIcon.SetActive(false);
            currentIcon = target;
        }

    }

    private void ChangeDescription(GameObject target)
    {
        if (currentDescription != target)
        {
            target.SetActive(true);
            if (currentDescription)
                currentDescription.SetActive(false);
            currentDescription = target;
        }

    }



}
