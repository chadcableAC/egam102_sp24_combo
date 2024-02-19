using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour
{
    public Button restartButton;

    public Canvas endCanvas;



    // Start is called before the first frame update

    private void OnEnable()
    {
        restartButton.onClick.AddListener(() => ButtonPress(restartButton));

    }

    private void ButtonPress(Button buttonPressed)
    {
        if (buttonPressed == restartButton)
        {

            UnityEngine.SceneManagement.SceneManager.LoadScene("kaine_jam3");
        }
    }
}
