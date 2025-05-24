using UnityEngine;

public class LockManager : BaseLockManager
{
    public LockDigit[] digits;
    public int[] correctCombination = { 3, 5, 4, 7 };

    public GameObject lockPanel;
    public GameObject unlockedPanel;

    public SFXPlayer sfxPlayer;

    public override void CheckCombination()
    {
        for (int i = 0; i < digits.Length; i++)
        {
            if (digits[i].GetValue() != correctCombination[i])
                return;
        }

        Debug.Log("¡Candado desbloqueado!");
        sfxPlayer.PlayDoor();
        if (lockPanel != null) lockPanel.SetActive(false);
        if (unlockedPanel != null) unlockedPanel.SetActive(true);
    }
}
