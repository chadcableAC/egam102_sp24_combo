using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaccoonMovement : MonoBehaviour
{
    public float speed;
    public float climbSpeed;

    public Vector2 direction;

    public Rigidbody2D rb;

    public GameObject ladder;
    public int foodHad;
    public bool canClimb;
    public SpriteRenderer coonRend;

    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coonRend = GetComponent<SpriteRenderer>();
        foodHad = 0;
    }
    void Update()
    {
        direction.x = Input.GetAxis("Horizontal");
        direction.y = Input.GetAxis("Vertical");

        if (rb.velocity.x >= 0.1f)
        {
            coonRend.flipX = true;
            //Debug.Log("flip?");

        }
        else if (rb.velocity.x <= -0.1f)
        {
            coonRend.flipX = false;
            //Debug.Log("flip!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if( collision.gameObject == ladder)
        {
            canClimb = true;
            
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if( col.gameObject == ladder && canClimb == true)
        {
            canClimb = false;
            
        }
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        if( col.gameObject.CompareTag("Food"))
        {
            foodHad++;
            Destroy(col.gameObject);
            
        }
    }
    private void FixedUpdate()
    {
        if (canClimb == false)
        {
            rb.velocity = new Vector2(direction.x * speed * Time.deltaTime, rb.velocity.y);
        }

        else if(canClimb == true)
        {
            rb.velocity = new Vector2(direction.x * speed * Time.deltaTime, direction.y * Time.deltaTime * climbSpeed);
        }
    }


    
}
