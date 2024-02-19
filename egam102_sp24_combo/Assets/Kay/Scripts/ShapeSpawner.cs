using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeSpawner : MonoBehaviour
{

    EgamMicrogameInstance microgameInstance;

    public List<Transform> spawnPoints = new List<Transform>();

    public List<Shape> shapePrefabList;

    public int spawnCount = 7;

    // Start is called before the first frame update
    void Start()
    {
        microgameInstance =
         FindObjectOfType<EgamMicrogameInstance>();

        List<Transform> usedPoints = new List<Transform>();

        for (int i = 0; i < spawnCount; i++)
        {
            int randomIndex = Random.Range(3, spawnPoints.Count);

            Transform spawnHandle = spawnPoints[randomIndex];

            while (usedPoints.Contains(spawnHandle))
            {
                randomIndex = Random.Range(0, spawnPoints.Count);
                spawnHandle = spawnPoints[randomIndex];
            }

            int randomShapeIndex = Random.Range(0, shapePrefabList.Count);
            Shape randomPrefab = shapePrefabList[randomShapeIndex];


            // Instantiate prefab
            Shape newShape = Instantiate(randomPrefab);
            // move the prefab to the spawn position
            newShape.moveHandles.position = spawnHandle.position;

            usedPoints.Add(spawnHandle);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
