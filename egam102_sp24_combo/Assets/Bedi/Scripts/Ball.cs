using UnityEngine;

public class Ball : MonoBehaviour
{
    public bool isCollected = false;
    private GameObject player;
    private Vector3 offset;
    private Vector2 movementDirection;
    public float moveSpeed = 2f; // Speed at which the ball moves

    // Bounds for the movement
    private float minX = -8f;
    private float maxX = 8f;
    private float minY = -4.5f;
    private float maxY = 4.5f;

    void Start()
    {
        // Set initial random direction
        movementDirection = Random.insideUnitCircle.normalized;
    }

    void Update()
    {
        if (!isCollected)
        {
            MoveRandomly();
        }
        else if (player != null)
        {
            // Follow the player with an offset
            transform.position = player.transform.position + offset;
        }
    }

    void MoveRandomly()
    {
        // Move ball in the random direction
        transform.position += (Vector3)movementDirection * moveSpeed * Time.deltaTime;

        // Check bounds and adjust direction if necessary
        if (transform.position.x < minX || transform.position.x > maxX)
        {
            movementDirection.x = -movementDirection.x;
            AdjustPositionToBounds();
        }
        if (transform.position.y < minY || transform.position.y > maxY)
        {
            movementDirection.y = -movementDirection.y;
            AdjustPositionToBounds();
        }
    }

    // Adjust the ball's position to ensure it's within bounds
    void AdjustPositionToBounds()
    {
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minX, maxX),
            Mathf.Clamp(transform.position.y, minY, maxY),
            transform.position.z
        );
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isCollected)
        {
            isCollected = true;
            player = collision.gameObject;

            // Set a random offset so balls don't overlap
            offset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);

            // Disable the ball's collider so it doesn't keep triggering other colliders
            GetComponent<Collider2D>().enabled = false;

            
            GameObject.Find("BallManager").GetComponent<BallManager>().AddBall(gameObject);
        }
    }
}
