using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class leverScript : MonoBehaviour
{
    public GameObject door;
    protected bool open = false;
    protected int i = 0;
    public int distance = 100;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (open && i < distance)
        {
            door.transform.position += Vector3.up * Time.deltaTime;
            i += 1;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (Input.GetButton("Interact"))
        {
            open = true;
            GetComponent<Animator>().Play("Lever");
        }

    }
}
