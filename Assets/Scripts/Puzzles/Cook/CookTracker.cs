using UnityEngine;
using UnityEngine.UI;

public class CookTracker : MonoBehaviour
{
    [Header("Panels")]
    public GameObject panel1;
    public GameObject panel2;
    public GameObject panel3;
    public GameObject panelFinal;

    [Header("Progreso visual")]
    public Image progresoImage; // Referencia al componente Image del panel
    public Sprite[] progresoSprites; // Sprites que representan 0, 1, 2 y 3 ingredientes

    private int ingredientCount = 0;

    public void AddIngredient()
    {
        ingredientCount++;

        // Asegura que no se salga del rango de sprites
        if (progresoSprites != null && progresoSprites.Length > ingredientCount - 1 && progresoImage != null)
        {
            progresoImage.sprite = progresoSprites[ingredientCount ];
        }

        if (ingredientCount >= 3)
        {
            ActivateFinalPanel();
        }
    }

    private void ActivateFinalPanel()
    {
        if (panel1 != null) panel1.SetActive(false);
        if (panel2 != null) panel2.SetActive(false);
        if (panel3 != null) panel3.SetActive(false);

        if (panelFinal != null) panelFinal.SetActive(true);
    }
}
