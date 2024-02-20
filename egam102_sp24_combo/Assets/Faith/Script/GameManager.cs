using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Faith
{
    public class GameManager : MonoBehaviour
    {


    
        //Once all stars have fall. When it is prayed more than the length of the List of Shooting star, then it will become false. 
        public static bool praying;

        //How many Stars will appear
        public static int spawncount;

        public GameObject replay;

        public SpriteRenderer handImage;
        public Sprite NotPray;
        public Sprite Pray;

        public GameObject Score;
        public GameObject PlayerScore;
        public GameObject ScoreA;
        public GameObject PlayerScoreA;


        public static int prayingNumber;
        float timeCount;


        EgamMicrogameInstance microgameInstace;


        //tlqkftlqkftlqkf

        private void Awake()
        {
            replay.SetActive(false);
        
            Score.SetActive(false);
            PlayerScore.SetActive(false);
            ScoreA.SetActive(false);
            PlayerScoreA.SetActive(false);

            praying = true;
            spawncount = Random.Range(1, 11);
            prayingNumber = 0;
        }

        // Start is called before the first frame update
        void Start()
        {
            microgameInstace = FindAnyObjectByType<EgamMicrogameInstance>();

        }

        // Update is called once per frame
        void Update()
        {
            if(timeCount > 1.3 && timeCount < 5)
            {
                prayforwish();
            }

            if(prayingNumber == spawncount && timeCount > 5)
            {
                microgameInstace.WinGame();
                replay.SetActive(true);

                Score.SetActive(true);
                PlayerScore.SetActive(true);
                ScoreA.SetActive(true);
                PlayerScoreA.SetActive(true);
            }

            else if(prayingNumber > spawncount && timeCount > 5.5)
            {
                replay.SetActive(true);

                Score.SetActive(true);
                PlayerScore.SetActive(true);
                ScoreA.SetActive(true);
                PlayerScoreA.SetActive(true);
            }

            else if (prayingNumber < spawncount && timeCount > 5.5)
            {
                replay.SetActive(true);

                Score.SetActive(true);
                PlayerScore.SetActive(true);
                ScoreA.SetActive(true);
                PlayerScoreA.SetActive(true);
            }

            timeCount += Time.deltaTime;
        }


        void prayforwish()
        {
            if (EgamInput.GetKeyDown(EgamInput.Key.Action))
            {
            prayingNumber++;
                handImage.sprite = NotPray;
            
            }

            else if (EgamInput.GetKeyUp(EgamInput.Key.Action))
            {
                handImage.sprite = Pray;
            }
        }

        //To win the game, players have to press space to pray. (After 1.3 second)
        //Players have to memorize the number of stars, and pray @ right amount.
        //If they do not pray the right amount, then its a lose.



    }
}
