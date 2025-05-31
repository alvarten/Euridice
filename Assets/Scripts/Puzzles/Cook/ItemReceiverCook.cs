using UnityEngine;
using UnityEngine.EventSystems;

public class ItemReceiverCook : MonoBehaviour, IDropHandler
{
    public string acceptedItemId;
    public PuzzleManager puzzleManager;
    public SFXPlayer sfxPlayer;

    public GameObject panelAntes;
    public GameObject panelDespues;

    public CookTracker cookTracker;

    public void OnDrop(PointerEventData eventData)
    {
        InventoryItemUI droppedItem = eventData.pointerDrag?.GetComponent<InventoryItemUI>();
        if (droppedItem != null && droppedItem.itemId == acceptedItemId)
        {
            Debug.Log("��tem correcto usado!");
            sfxPlayer?.PlayPick();
            Destroy(droppedItem.gameObject);

            puzzleManager?.ActivarPanelResuelto(panelAntes, panelDespues);
            cookTracker?.AddIngredient(); // A�adimos uno al contador
        }
        else
        {
            Debug.Log("�tem incorrecto.");
        }
    }
}
