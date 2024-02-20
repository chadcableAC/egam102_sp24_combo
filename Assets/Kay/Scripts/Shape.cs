using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour
{
    public static Shape Instance;

    public Transform moveHandles;

    public Rigidbody2D shapeRb;

    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        //Rotate the asteroid to a random rotation!
        transform.Rotate(0, 0, Random.Range(0, 360));

        //Apply a force to push the asteroid foward
        shapeRb.AddForce(transform.up * speed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            shapeRb.AddForce((transform.up * -1) * speed * 2);
        }
    }
}
