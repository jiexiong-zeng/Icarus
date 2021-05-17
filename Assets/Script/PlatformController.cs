using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public float waiting = 0.02f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            StartCoroutine(FallTimer());
        }
        
    }
    IEnumerator FallTimer()
    {
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().gravityScale += 4;
        yield return new WaitForSeconds(waiting);
        GetComponent<CircleCollider2D>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<Rigidbody2D>().gravityScale -= 4;
    }
}
