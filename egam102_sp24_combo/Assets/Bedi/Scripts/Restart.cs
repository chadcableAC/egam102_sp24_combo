using UnityEngine;
using UnityEngine.SceneManagement; // Required for loading scenes

public class Restart : MonoBehaviour
{
    public void RestartGame()
    {
        // Reloads the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
