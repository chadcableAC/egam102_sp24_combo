using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThingSpawner : MonoBehaviour
{
    public List<Transform> spawnPoints = new List<Transform>();

    public int spawnCount;

    public float repeatSpawnTimer = 5;
    public bool isRepeatSpawnTimerRunning;

    public bool canSpawn;

    public Transform enemyHandle;

    // add reference to prefab to be spawned

    public EnemyBehavior blockThis;


    // Start is called before the first frame update
    void Start()
    {
        canSpawn = true;

        isRepeatSpawnTimerRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canSpawn == true)
        {
            Spawn();
        }
    }

    public void Spawn()
    {
        List<Transform> usedPoints = new List<Transform>();
        spawnCount = Random.Range(2, 4);

        for (int i = 0; i < spawnCount; i++) 
        { 
            int randomIndex = Random.Range(0, spawnPoints.Count);
            
            Transform spawnHandle = spawnPoints[randomIndex];

            while (usedPoints.Contains(spawnHandle))
            {
                randomIndex = Random.Range(0, spawnPoints.Count);
                spawnHandle = spawnPoints[randomIndex];
            }

            // Instantiate prefab
            EnemyBehavior newEnemyBehavior = Instantiate(blockThis);
            // move the prefab to the spawn position
            newEnemyBehavior.moveHandles.position = spawnHandle.position;
            newEnemyBehavior.enemyHandle = enemyHandle;

            usedPoints.Add(spawnHandle);
        }

        canSpawn = false;
    }
}
