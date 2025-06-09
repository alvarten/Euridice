using UnityEngine;
using TMPro;

public class CodePad : MonoBehaviour
{
    public SFXPlayer sfxPlayer;
    [SerializeField] private string correctCode = "1234";
    [SerializeField] private TextMeshProUGUI displayText;

    [Header("Acciones al desbloquear")]
    [SerializeField] private GameObject objectToHide;
    [SerializeField] private GameObject objectToShow;
    [SerializeField] private GameObject panelToHide;
    [SerializeField] private PuzzleManager puzzleManager;

    private int progress = 0;
    private string currentInput = "";
    private bool isUnlocked = false;

    public void PressKey(string digit)
    {
        if (isUnlocked) return;

        if (digit == correctCode[progress].ToString())
        {
            currentInput += digit;
            progress++;

            if (progress == correctCode.Length)
            {
                isUnlocked = true;
                OnUnlocked();
            }
        }
        else
        {
            ResetCode();
            sfxPlayer.PlayError();
        }

        UpdateDisplay();
    }

    private void ResetCode()
    {
        currentInput = "";
        progress = 0;
    }

    private void UpdateDisplay()
    {
        if (isUnlocked)
        {
            displayText.text = "UNLOCKED";
        }
        else
        {
            displayText.text = currentInput.PadRight(correctCode.Length, '_');
        }
    }

    private void OnUnlocked()
    {
        Debug.Log("¡Código correcto!");

        // Ocultar y mostrar objetos si están asignados
        if (objectToHide != null)
            objectToHide.SetActive(false);

        if (objectToShow != null)
            objectToShow.SetActive(true);

        // Llamar al PuzzleManager si está asignado
        if (puzzleManager != null)
            puzzleManager.TogglePuzzlePanel(panelToHide);
    }
}
