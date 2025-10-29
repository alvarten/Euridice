using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject optionsPanel;
    public GameObject player;
    public PuzzleManager puzzleManager;

    private bool isPaused = false;
    private bool ignoreNextEscape = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Si hay un puzzle abierto, pedimos al PuzzleManager que lo cierre y no abrimos el menú.
            if (puzzleManager != null && puzzleManager.isPuzzleOpen)
            {
                puzzleManager.CloseCurrentPuzzle();
                return;
            }
            else if (optionsPanel.activeSelf)
            {
                // Si estamos en opciones, volvemos al panel de pausa
                optionsPanel.SetActive(false);
                pausePanel.SetActive(true);
            }
            else
            {
                // En cualquier otro caso abrimos el menu de pausa
                TogglePause();
            }
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pausePanel.SetActive(isPaused);
        TogglePlayerMovement(!isPaused);
    }

    public void ResumeGame()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        TogglePlayerMovement(true);
    }

    public void OpenOptions()
    {
        pausePanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void QuitToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    private void TogglePlayerMovement(bool canMove)
    {
        if (player != null)
        {
            var controller = player.GetComponent<PlayerController>();
            if (controller != null)
            {
                controller.canMove = canMove;
            }
        }
    }
}
