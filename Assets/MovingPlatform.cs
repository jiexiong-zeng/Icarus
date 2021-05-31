using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform pos1, pos2;
    public float speed;
    private bool stop = false;
    Vector3 nextPos;

    void Start()
    {
        nextPos = pos2.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position == pos1.position)
        {
            nextPos = pos2.position;
            StartCoroutine(Wait());
        }
        if (transform.position == pos2.position)
        {
            nextPos = pos1.position;
            StartCoroutine(Wait());
        }


        if (!stop)
        {
            transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
        }
    }

    IEnumerator Wait()
    {
        stop = true;
        yield return new WaitForSeconds(2f);
        stop = false;
        transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * 0.1f * Time.deltaTime);
    }
}
    