using UnityEngine;

public class InventoryAdd : MonoBehaviour
{
    [Header("Datos del objeto")]
    public string itemId = "ObjetoNuevo";
    public Sprite itemIcon;

    [Header("Comportamiento")]
    public bool destroyAfterAdd = false;

    public void AddToInventory()
    {      
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddItem(itemIcon, itemId);
            Debug.Log($"Objeto '{itemId}' a�adido al inventario.");
        }
        else
        {
            Debug.LogWarning("InventoryManager.Instance no est� asignado.");
        }

        if (destroyAfterAdd)
            Destroy(gameObject);
    }
}
