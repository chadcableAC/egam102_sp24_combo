using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public List<Transform> falling = new List<Transform>();

    public Star starPrefab;

    public float spawnBtwn;

    // Start is called before the first frame update
    void Start()
    {
        List<Transform> usedFallPosition = new List<Transform>();


        for (int i = 0; i < GameManager.spawncount; i++)
        {
            int randomIndex = Random.Range(0, falling.Count);
            Transform fallPos = falling[randomIndex];

            while (usedFallPosition.Contains(fallPos))
            {
               randomIndex = Random.Range(0, falling.Count);
               fallPos = falling[randomIndex];
            }


            Star newStar = Instantiate(starPrefab);
            newStar.moveHandle.position = fallPos.position;

            usedFallPosition.Add(fallPos);

        }
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
