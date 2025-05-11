using UnityEngine;
using UnityEngine.UI;

public class LockDigit : MonoBehaviour
{
    public Image digitImage; // La imagen que representa el número
    public Sprite[] digitSprites; // 10 sprites, uno por cada número del 0 al 9

    private int currentValue = 0;

    public void Increase()
    {
        currentValue = (currentValue + 1) % 10;
        UpdateImage();
        LockManager.Instance.CheckCombination();
    }

    public void Decrease()
    {
        currentValue = (currentValue - 1 + 10) % 10; // wrap-around hacia atrás
        UpdateImage();
        LockManager.Instance.CheckCombination();
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
