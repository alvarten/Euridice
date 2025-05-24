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
        Debug.Log("Opciones abiertas (aquí irá el menú de opciones)");
        // Aquí puedes abrir un panel de opciones más adelante
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Juego cerrado");
    }
}
