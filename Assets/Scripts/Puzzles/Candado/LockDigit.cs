using UnityEngine;
using UnityEngine.UI;

public class LockDigit : MonoBehaviour
{
    public Image digitImage; // La imagen que representa el n�mero
    public Sprite[] digitSprites; // 10 sprites, uno por cada n�mero del 0 al 9
    public SFXPlayer sfxPlayer;
    private int currentValue = 0;

    public void Increase()
    {
        currentValue = (currentValue + 1) % 10;
        UpdateImage();
        LockManager.Instance.CheckCombination();
        
        sfxPlayer.PlayLock();
    }

    public void Decrease()
    {
        currentValue = (currentValue - 1 + 10) % 10;
        UpdateImage();
        LockManager.Instance.CheckCombination();
        sfxPlayer.PlayLock();
    }

    void UpdateImage()
    {
        digitImage.sprite = digitSprites[currentValue];
    }

    public int GetValue()
    {
        return currentValue;
    }
}
