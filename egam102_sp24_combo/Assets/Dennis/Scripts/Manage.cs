using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager2 : MonoBehaviour
{
    public string SceneName;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public void ChangeScene()
    {
        SceneManager.LoadScene(SceneName);
    }
}