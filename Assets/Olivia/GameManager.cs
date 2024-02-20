using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject pimplePrefab;
    private List<GameObject> pimples = new List<GameObject>();

    EgamMicrogameInstance microgameInstance;

    Animator m_Animator;

    // Start 
    void Start()
    {
        microgameInstance = FindObjectOfType<EgamMicrogameInstance>();

        GameObject animatorObject = GameObject.FindGameObjectWithTag("Face");
        if (animatorObject != null)
        {
            m_Animator = animatorObject.GetComponent<Animator>();
        }

        SpawnPimples();

    }

    // Update 
    void Update()
    {
        if (pimples.Count == 0)
        {
            Debug.Log("game over, all pimples popped!");

            if (m_Animator != null)
            {
                m_Animator.SetBool("Happy", true);
            }

            enabled = false;

            microgameInstance.WinGame();


            //GAME RESTARTS WHEN WON

            Invoke("RestartScene", 2f);
        }


        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.tag == "Pimple")
            {
                Destroy(hit.collider.gameObject);
                pimples.Remove(hit.collider.gameObject);
            }
            Debug.Log("click");
        }

        //GAME RESTART WHEN PRESSING ESCAPE

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            RestartScene();
        }
    }

    //spawing pimples
    void SpawnPimples()
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 randomPos = new Vector3(Random.Range(-3f, 3f), Random.Range(-2f, 5f), 0f);

            GameObject pimple = Instantiate(pimplePrefab, randomPos, Quaternion.identity);

            pimples.Add(pimple); // adding instantiated pimple to the list
        }
    }

    //GAME RESTART CODE
    void RestartScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}
