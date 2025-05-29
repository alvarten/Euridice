using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    // Reinicia la escena actual
    public void RestartCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    // Carga directamente la escena "Escena1"
    public void LoadEscena1()
    {
        SceneManager.LoadScene(2);
    }
}

