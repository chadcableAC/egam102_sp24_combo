using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LotteryGame : MonoBehaviour
{
    public int maxAttempts = 3; 
    public int targetSum = 50; 

    public AudioClip successSound;
    public AudioClip failSound; 

    private int attemptsLeft; 
    private int currentSum; 
    private List<int> numbers; 
    private EgamMicrogameInstance microgameInstance; 

    public int AttemptsLeft => attemptsLeft; 
    public int CurrentSum => currentSum; 

    private AudioSource successAudioSource; 
    private AudioSource failAudioSource; 

    private void Start()
    {
        attemptsLeft = maxAttempts;
        currentSum = 0;
        numbers = new List<int>();

        microgameInstance = FindObjectOfType<EgamMicrogameInstance>();

        successAudioSource = gameObject.AddComponent<AudioSource>();
        failAudioSource = gameObject.AddComponent<AudioSource>();

        successAudioSource.clip = successSound;
        failAudioSource.clip = failSound;
    }

    private void PlaySuccessSound()
    {
        successAudioSource.Play(); 
    }

    private void PlayFailSound()
    {
        failAudioSource.Play(); 
    }

    private void Update()
    {
        if (EgamInput.GetKey(EgamInput.Key.Action)) 
        {
            if (attemptsLeft > 0)
            {
                int randomNumber = Random.Range(1, 26); 
                numbers.Add(randomNumber); 
                currentSum += randomNumber; 

                attemptsLeft--;

                if (currentSum > targetSum)
                {
                    PlaySuccessSound(); 
                    microgameInstance.WinGame();
                }
                else if (attemptsLeft == 0)
                {
                    PlayFailSound();
                    microgameInstance.LoseGame();
                }
            }
        }
    }

    public List<int> GetNumbers()
    {
        return numbers;
    }
}