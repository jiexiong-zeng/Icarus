using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningEffectScript : MonoBehaviour
{
    private Animator animator;
    // Start is called before the first frame update
    const string EFFECT1 = "AttackEffect1";
    const string EFFECT2 = "AttackEffect2";
    const string EFFECT3 = "AttackEffect3";
    void Start()
    {
        animator = GetComponent<Animator>();
        int random = Random.Range(1, 4);
        switch (random)
        {
            case 1:
                animator.Play(EFFECT1);
                break;
            case 2:
                animator.Play(EFFECT2);
                break;
            case 3:
                animator.Play(EFFECT3);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
