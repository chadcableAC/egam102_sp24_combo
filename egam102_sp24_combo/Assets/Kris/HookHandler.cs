using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class HookHandler : MonoBehaviour
{
    public BoxCollider hookBox;

    public float score = 0;

    public TMP_Text scoreText;

    public GameObject restartingText;

    EgamMicrogameInstance microgameInstance;


    // Start is called before the first frame update
    void Start()
    {
        hookBox = GetComponent<BoxCollider>();
        microgameInstance = FindObjectOfType<EgamMicrogameInstance>();

    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "POINTS: " + score;
        if (score >= 50)
        {
            StartCoroutine(winGame1());
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided");
        if (other.gameObject.tag == "fish1")
        {
            Destroy(other.gameObject);
            score++;
        }
        else if (other.gameObject.tag == "fish2")
        {
            Destroy(other.gameObject);
            for (int i = 0; i < 3; i++)
            {
                score++;
            }
        }
        else if (other.gameObject.tag == "fish3")
        {
            Destroy(other.gameObject);
            for (int i = 0; i < 10; i++) 
            {
                score++;
            }
        }
    }

    IEnumerator winGame1()
    {
        microgameInstance.WinGame();
        restartingText.SetActive(true);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
