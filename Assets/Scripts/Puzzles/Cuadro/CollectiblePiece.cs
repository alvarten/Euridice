using UnityEngine;
using UnityEngine.UI;

public class CollectiblePiece : MonoBehaviour
{
    [Header("Datos del coleccionable")]
    public string itemId = "PiezaEspada"; // Clave �nica
    public Sprite itemIcon;              // Icono a mostrar en el inventario
    public bool usePlayerPrefs = true;   // �Guardar como recogido?

    [Header("Opciones visuales")]
    public bool destroyOnCollect = true; // O usar SetActive(false)

    void Start()
    {        
        // Si ya se recogi� anteriormente y queremos recordar eso
        //if (usePlayerPrefs && PlayerPrefs.GetInt(itemId, 0) == 1)
        //{
        //    gameObject.SetActive(false);
        //}
    }

    // Llama esto desde un bot�n, o al hacer clic, trigger, etc.
    public void OnClickCollect()
    {
        if (usePlayerPrefs)
        {
            PlayerPrefs.SetInt(itemId, 1);
            PlayerPrefs.Save();
        }

        // A�adir al inventario
        InventoryManager.Instance.AddItem(itemIcon, itemId);
        Debug.Log("�Va a destruirse!");
        // Ocultar objeto recogido
        if (destroyOnCollect) {
            Debug.Log("�Se destruye!");
            Destroy(gameObject);
        }
        else
            gameObject.SetActive(false);
    }
}
