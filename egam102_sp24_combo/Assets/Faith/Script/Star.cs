using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public Transform moveHandle;

    public Vector3 originalPosition;
    public int ShootingMove = 2;
    public float StopFalling;
    public float transparent;

    public SpriteRenderer Spr;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = moveHandle.position;
        transparent = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (StopFalling > 1 && StopFalling < 1.3)
        {
            transform.Translate(Vector3.left * Time.deltaTime * 5);
            transform.Translate(Vector3.down * Time.deltaTime * 5);
            Spr.color = new Color(1, 1, 1, transparent -= 0.05f);
        }

        else if (StopFalling > 1.3)
        {
            Destroy(gameObject);
        }

        StopFalling += Time.deltaTime;
    }
}
