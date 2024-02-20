using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerHandler : MonoBehaviour
{
    public List<GameObject> fishList = new List<GameObject>();

    int randomInt;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnFish());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }

    IEnumerator spawnFish()
    {
        while (true)
        {
            randomInt = Random.Range(0, fishList.Count);
            Mathf.Round(randomInt);
            Instantiate(fishList[randomInt]);
            yield return new WaitForSeconds(.5f);
        }
    }
}
