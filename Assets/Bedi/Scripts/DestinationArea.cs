using UnityEngine;

public class DestinationArea : MonoBehaviour
{
    // Reference to the BallManager to check the collected balls
    public BallManager ballManager;
    EgamMicrogameInstance microgameInstance;

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Player") && ballManager.CheckWinCondition())
        {
            Debug.Log("Player has brought one of each type of ball. Win!");
           
            OnWinCondition();
        }

    }

    void Start()
    {
        microgameInstance =
           FindObjectOfType<EgamMicrogameInstance>();
    }
    public void OnWinCondition()
    {
        microgameInstance.WinGame();
    }

}

