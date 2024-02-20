using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Movement speed of the player

    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        Vector3 moveDirection = Vector3.zero; // Initialize movement direction to zero

        // Check for right movement
        if (EgamInput.GetKey(EgamInput.Key.Right))
        {
            moveDirection += Vector3.right; // Move right
        }

        if (EgamInput.GetKey(EgamInput.Key.Left))
        {
            moveDirection += Vector3.left; // Move left
        }
        if (EgamInput.GetKey(EgamInput.Key.Up))
        {
            moveDirection += Vector3.up; // Move up
        }
        if (EgamInput.GetKey(EgamInput.Key.Down))
        {
            moveDirection += Vector3.down; // Move down
        }

        // Apply movement
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
}
