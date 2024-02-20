using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumpItem : MonoBehaviour
{
    public Rigidbody2D rb;
    public float minX, maxX, minY, maxY;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TrashLauncher()
    {

        Invoke(nameof(ShootTrash), Random.Range(0.1f, 1));
    }

    public void ShootTrash()
    {
        rb.AddForce(new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY)), ForceMode2D.Impulse);

    }
}
