using UnityEngine;
using UnityEngine.UI;

public class CollectiblePiece : MonoBehaviour
{
    public SFXPlayer sfxPlayer;
    [Header("Datos del coleccionable")]
    public string itemId = "PiezaEspada"; // Clave única
    public Sprite itemIcon;              // Icono a mostrar en el inventario
    public bool usePlayerPrefs = true;   // Para guardarlo como recogido en un futuro

    [Header("Opciones visuales")]
    public bool destroyOnCollect = true; // O usar SetActive(false)

    void Start()
    {        
        
    }

    // Funcion para anadir el objeto al inventario
    public void OnClickCollect()
    {
        if (usePlayerPrefs)
        {
            PlayerPrefs.SetInt(itemId, 1);
            PlayerPrefs.Save();
        }

        // Añadir al inventario
        InventoryManager.Instance.AddItem(itemIcon, itemId);
        sfxPlayer.PlayPick();
        // Ocultar objeto recogido o destruierlo
        if (destroyOnCollect) {
            //Debug.Log("¡Se destruye!");
            Destroy(gameObject);
        }
        else
            gameObject.SetActive(false);
    }
}
