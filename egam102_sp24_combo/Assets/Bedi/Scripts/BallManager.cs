using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    public GameObject[] ballTypes; // Assign your 3 ball prefabs here in the inspector
    public int ballsPerType = 3;
    private List<GameObject> collectedBalls = new List<GameObject>();
    

    void Start()
    {
        SpawnBalls();


    }

    void SpawnBalls()
    {
        for (int i = 0; i < ballTypes.Length; i++)
        {
            for (int j = 0; j < ballsPerType; j++)
            {
                Vector2 spawnPosition = new Vector2(Random.Range(-8f, 8f), Random.Range(-4.5f, 4.5f));
                Instantiate(ballTypes[i], spawnPosition, Quaternion.identity);
            }
        }
    }

    public void AddBall(GameObject ball)
    {
        if (!collectedBalls.Contains(ball))
        {
            collectedBalls.Add(ball);
            CheckWinCondition();
        }
    }

    public bool CheckWinCondition()
    {
        bool hasAllTypes = true;
        // Assuming each type of ball has a distinct tag assigned in Unity
        foreach (var ballPrefab in ballTypes)
        {
            bool found = false;
            foreach (var ball in collectedBalls)
            {
                if (ball.tag == ballPrefab.tag)
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                hasAllTypes = false;
                break;
            }
        }

        if (hasAllTypes)
        {
            Debug.Log("Win Condition Met!");
            // Implement win logic here, such as triggering a victory screen or animation
        }
        return hasAllTypes; // Return the result
    }

}
