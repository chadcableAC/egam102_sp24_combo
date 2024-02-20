using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI myPrayText;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = GameManager.spawncount.ToString();
        myPrayText.text = GameManager.prayingNumber.ToString();
    }
}
