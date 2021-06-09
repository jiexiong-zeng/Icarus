using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAggro : MonoBehaviour
{

    SkeletonController skeletonCtrl;

    void Start()
    {
        skeletonCtrl = GetComponentInParent<SkeletonController>();
    }

    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            skeletonCtrl.aggroed = true;
        }
    }


}
