using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSignController : MonoBehaviour
{
    EgamMicrogameInstance microgameInstance;

    public Image image;

    public Sprite equalSign;
    public Sprite greaterThan;
    public Sprite lessThan;

    public AudioSource correctAudio;
    public AudioSource incorrectAudio;
    public AudioSource switchAudio;

    public bool signSelected = false;

    public enum SignStates
    {
        Equal,                  //0
        GreaterThan,            //1
        LessThan,               //2
    }

    public SignStates previousState;
    public SignStates currentState;

    // Start is called before the first frame update
    void Start()
    {
        microgameInstance =
         FindObjectOfType<EgamMicrogameInstance>();
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {

            case SignStates.Equal:
                UpdateEqual();
                
                break;
            case SignStates.GreaterThan:
                UpdateGreaterThan();
                
                break;
            case SignStates.LessThan:
                UpdateLessThan();
                
                break;
        }


    }

    private void UpdateEqual()
    {
        image.sprite = equalSign;
        

        // left and right arrows to switch between signs
        if (EgamInput.GetKeyDown(EgamInput.Key.Left) && signSelected == false)
        {
            // Do code for "left" (right arrow, D, etc)
            currentState = SignStates.LessThan;
            switchAudio.Play();
        }
        else if (EgamInput.GetKeyDown(EgamInput.Key.Right) && signSelected == false)
        {
            // Do code for "right" (right arrow, D, etc)
            currentState = SignStates.GreaterThan;
            switchAudio.Play();
        }

        // space to lock in sign, then check to see if correct
        if (EgamInput.GetKeyDown(EgamInput.Key.Action))
        {
            // check to see if value is equal
            if (GameObject.FindGameObjectsWithTag("Triangle").Length == GameObject.FindGameObjectsWithTag("Circle").Length)
            {
                microgameInstance.WinGame();
                correctAudio.Play();
                signSelected = true;
            }
            else
            {
                microgameInstance.LoseGame();
                incorrectAudio.Play();
                signSelected = true;
            }
        }
    }

    private void UpdateGreaterThan()
    {
        image.sprite = greaterThan;

        // left and right arrows to switch between signs
        if (EgamInput.GetKeyDown(EgamInput.Key.Left) && signSelected == false)
        {
            // Do code for "left" (right arrow, D, etc)
            currentState = SignStates.Equal;
            switchAudio.Play();
        }
        else if (EgamInput.GetKeyDown(EgamInput.Key.Right) && signSelected == false)
        {
            // Do code for "right" (right arrow, D, etc)
            currentState = SignStates.LessThan;
            switchAudio.Play();
        }

        // space to lock in sign, then check to see if correct
        if (EgamInput.GetKeyDown(EgamInput.Key.Action))
        {
            // check to see if value is equal
            if (GameObject.FindGameObjectsWithTag("Triangle").Length >= GameObject.FindGameObjectsWithTag("Circle").Length)
            {
                microgameInstance.WinGame();
                correctAudio.Play();
                signSelected = true;
            }
            else
            {
                microgameInstance.LoseGame();
                incorrectAudio.Play();
                signSelected = true;
            }
        }
    }
    private void UpdateLessThan()
    {
        image.sprite = lessThan;

        // left and right arrows to switch between signs
        if (EgamInput.GetKeyDown(EgamInput.Key.Left) && signSelected == false)
        {
            // Do code for "left" (right arrow, D, etc)
            currentState = SignStates.GreaterThan;
            switchAudio.Play();
        }
        else if (EgamInput.GetKeyDown(EgamInput.Key.Right) && signSelected == false)
        {
            // Do code for "right" (right arrow, D, etc)
            currentState = SignStates.Equal;
            switchAudio.Play();
        }

        // space to lock in sign, then check to see if correct
        if (EgamInput.GetKeyDown(EgamInput.Key.Action))
        {
            // check to see if value is equal
            if (GameObject.FindGameObjectsWithTag("Triangle").Length <= GameObject.FindGameObjectsWithTag("Circle").Length)
            {
                microgameInstance.WinGame();
                correctAudio.Play();
                signSelected = true;
            }
            else
            {
                microgameInstance.LoseGame();
                incorrectAudio.Play();
                signSelected = true;
            }
        }
    }
}
