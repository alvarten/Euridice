using UnityEngine;
using UnityEngine.EventSystems;

public class ItemReceiver : MonoBehaviour, IDropHandler
{
    public string acceptedItemId;

    public void OnDrop(PointerEventData eventData)
    {
        InventoryItemUI droppedItem = eventData.pointerDrag?.GetComponent<InventoryItemUI>();
        if (droppedItem != null && droppedItem.itemId == acceptedItemId)
        {
            Debug.Log("¡Ítem correcto usado!");

            // Destruir el objeto del inventario
            Destroy(droppedItem.gameObject);


        }
        else
        {
            Debug.Log("Ítem incorrecto.");
        }
    }
}
