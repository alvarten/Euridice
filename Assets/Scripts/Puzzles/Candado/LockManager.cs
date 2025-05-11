using UnityEngine;

public class LockManager : MonoBehaviour
{
    public static LockManager Instance;

    public LockDigit[] digits;
    public int[] correctCombination = { 3, 5, 4, 7 };

    public GameObject lockPanel;       // Panel que contiene el candado
    public GameObject unlockedPanel;   // Panel que aparece al desbloquear

    void Awake()
    {
        Instance = this;
    }

    public void CheckCombination()
    {
        for (int i = 0; i < digits.Length; i++)
        {
            if (digits[i].GetValue() != correctCombination[i])
                return;
        }

        Debug.Log("¡Candado desbloqueado!");
        if (lockPanel != null) lockPanel.SetActive(false);
        if (unlockedPanel != null) unlockedPanel.SetActive(true);
        
            
    }
}
