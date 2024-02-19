using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainController : MonoBehaviour
{
    public EgamMicrogameInstance microInst;
    public RaccoonMovement playerScript;
    public Button restartButton;


    // Start is called before the first frame update
    void Start()
    {
        microInst = FindObjectOfType<EgamMicrogameInstance>();
        playerScript = FindObjectOfType<RaccoonMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if( playerScript.foodHad >= 2)
        {
            Debug.Log("Winner");
            microInst.WinGame();
            restartButton.gameObject.SetActive(true);

        }

        if( microInst._isGameOver == true)
        {
            restartButton.gameObject.SetActive(true);
        }
    }
}
