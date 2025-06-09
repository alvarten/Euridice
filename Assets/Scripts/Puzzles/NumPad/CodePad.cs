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
    
    private string currentInput = "";
    private bool isUnlocked = false;

    public void PressKey(string digit)
    {
        if (isUnlocked || currentInput.Length >= correctCode.Length) return;

        currentInput += digit;
        UpdateDisplay();

        if (currentInput.Length == correctCode.Length)
        {
            if (currentInput == correctCode)
            {
                isUnlocked = true;
                OnUnlocked();
            }
            else
            {
                sfxPlayer.PlayError();
                ResetCode();
                UpdateDisplay();
            }
        }
    }

    private void ResetCode()
    {
        currentInput = "";
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
