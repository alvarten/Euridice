using UnityEngine;
using UnityEngine.EventSystems;

public class ItemReceiver : MonoBehaviour, IDropHandler
{
    public string acceptedItemId;
    public PuzzleManager puzzleManager;

    public GameObject panelAntes;    // Asigna en el Inspector
    public GameObject panelDespues;  // Asigna en el Inspector

    public void OnDrop(PointerEventData eventData)
    {
        InventoryItemUI droppedItem = eventData.pointerDrag?.GetComponent<InventoryItemUI>();
        if (droppedItem != null && droppedItem.itemId == acceptedItemId)
        {
            Debug.Log("¡Ítem correcto usado!");
            Destroy(droppedItem.gameObject);

            if (puzzleManager != null)
            {
                puzzleManager.ActivarPanelResuelto(panelAntes, panelDespues);
            }
        }
        else
        {
            Debug.Log("Ítem incorrecto.");
        }
    }
}
