using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FIshHandler : MonoBehaviour
{
    public float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        //Random move speed
        if (gameObject.tag == "fish1")
        {
            moveSpeed = Random.Range(-5f, -10f);
        }
        else if (gameObject.tag == "fish2")
        {
            moveSpeed = Random.Range(-9f, -18f);
        }
        else if (gameObject.tag == "fish3")
        {
            moveSpeed = Random.Range(-18f, -25f);
        }

        //random spawn point
        transform.position = new Vector3(12, Random.Range(-3.5f, 3.5f), 0);
    }

    // Update is called once per frame
    void Update()
    {
        //move fish
        transform.position = new Vector2(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y);

        if (transform.position.x < -12f)
        {
            Destroy(gameObject);
        }
    }
}
