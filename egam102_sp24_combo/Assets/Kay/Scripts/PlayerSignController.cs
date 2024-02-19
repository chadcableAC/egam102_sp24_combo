using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSignController : MonoBehaviour
{
    EgamMicrogameInstance microgameInstance;

    public Sprite equalSign;
    public Sprite greaterThan;
    public Sprite lessThan;

    public bool signSelected;

    public enum SignStates
    {
        Equal,                  //0
        GreaterThan,            //1
        LessThan,               //2
        //EqualSelected,          //3
        //GreaterThanSelected,    //4
        //LessThanSelected        //5
    }

    public SignStates previousState;
    public SignStates currentState;

    // Start is called before the first frame update
    void Start()
    {
        microgameInstance =
         FindObjectOfType<EgamMicrogameInstance>();
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

        // left and right arrows to switch between signs
        if (Input.GetKeyDown(KeyCode.LeftArrow) && signSelected == false)
        {
            // Do code for "left" (right arrow, D, etc)
            currentState = currentState++;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && signSelected == false)
        {
            // Do code for "right" (right arrow, D, etc)
            currentState = currentState--;
        }


        // space to lock in sign, then check to see if correct
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // check to see if value is equal
            if (GameObject.FindGameObjectsWithTag("Triangle") == GameObject.FindGameObjectsWithTag("Circle"))
            {
                microgameInstance.WinGame();
            }
            else
            {
                microgameInstance.LoseGame();
            }
        }
    }

    private void UpdateGreaterThan()
    {

    }
    private void UpdateLessThan()
    {

    }
    //private void UpdateEqualSelected()
    //{

    //}
    //private void UpdateGreaterThanSelected()
    //{

    //}
    //private void UpdateLessThanSelected()
    //{

    //}
}
