using UnityEngine;

public class DoorLockManager : BaseLockManager
{
    public LockDigit[] digits;
    public int[] correctCombination = { 3, 5, 4, 7 };

    public GameObject lockPanel;
    public PuzzleManager puzzleManager;
    public GameObject interactuableAntes;
    public GameObject interactuableDespues;

    public SFXPlayer sfxPlayer;

    public override void CheckCombination()
    {
        for (int i = 0; i < digits.Length; i++)
        {
            if (digits[i].GetValue() != correctCombination[i])
                return;
        }

        Debug.Log("¡Puerta desbloqueada!");
        sfxPlayer.PlayLock();

        if (puzzleManager != null && lockPanel != null)
        {
            puzzleManager.TogglePuzzlePanel(lockPanel);
        }

        if (interactuableAntes != null) interactuableAntes.SetActive(false);
        if (interactuableDespues != null) interactuableDespues.SetActive(true);
    }
}
