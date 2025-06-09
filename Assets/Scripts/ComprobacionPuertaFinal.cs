using UnityEngine;
using UnityEngine.SceneManagement;

public class ComprobacionPuertaFinal : MonoBehaviour
{
    [Header("Nombre del objeto necesario en el inventario")]
    public string itemNecesario = "LlaveFinal";
    public SFXPlayer sfxPlayer;

    public void Check()
    {
        if (InventoryManager.Instance != null && InventoryManager.Instance.HasItem(itemNecesario))
        {
            Debug.Log($"El jugador tiene el objeto requerido: {itemNecesario}");
            SceneManager.LoadScene(4);
        }
        else
        {
            sfxPlayer?.PlayError();
            Debug.Log($"El jugador **NO** tiene el objeto necesario: {itemNecesario}");
        }
    }
}
