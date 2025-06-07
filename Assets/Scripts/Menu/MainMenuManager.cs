using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        int lastScene = PlayerPrefs.GetInt("LastScene", 1);
        SceneManager.LoadScene(lastScene); 
    }

    public void OpenOptions()
    {
        Debug.Log("Opciones abiertas (aqu� ir� el men� de opciones)");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Juego cerrado");
    }
}
