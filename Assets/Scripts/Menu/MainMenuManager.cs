using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("TutorialNuevo");
    }

    public void OpenOptions()
    {
        Debug.Log("Opciones abiertas (aqu� ir� el men� de opciones)");
        // Aqu� puedes abrir un panel de opciones m�s adelante
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Juego cerrado");
    }
}
