using UnityEngine;

public class CookTracker : MonoBehaviour
{
    [Header("Panels")]
    public GameObject panel1;
    public GameObject panel2;
    public GameObject panel3;
    public GameObject panelFinal;

    private int ingredientCount = 0;

    public void AddIngredient()
    {
        ingredientCount++;

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
