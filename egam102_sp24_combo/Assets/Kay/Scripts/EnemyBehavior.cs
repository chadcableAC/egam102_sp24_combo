using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyBehavior : MonoBehaviour
{
    public float moveSpeed;
    // add list of different speeds to choose from

    public Transform enemyHandle;
    public Transform moveHandles;

    public static EnemyBehavior instance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Move towards this position
        Vector3 targetPosition = enemyHandle.position;

        //to find the direction from A to B = B - A
        Vector3 delta = targetPosition - moveHandles.position;
        Vector3 moveDirection = delta.normalized;

        moveHandles.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Protect This"))
        {

        }
    }
}
