using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public GameObject player; // Asigna el jugador desde el Inspector

    public void TogglePuzzlePanel(GameObject panel)
    {
        if (panel == null) return;

        bool isActive = panel.activeSelf;
        panel.SetActive(!isActive);

        if (player != null)
        {
            var controller = player.GetComponent<PlayerController>();
            if (controller != null)
            {
                controller.enabled = !panel.activeSelf;
            }
        }

        // También puedes desbloquear o bloquear el cursor si es necesario:
        //Cursor.visible = panel.activeSelf;
        //Cursor.lockState = panel.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
