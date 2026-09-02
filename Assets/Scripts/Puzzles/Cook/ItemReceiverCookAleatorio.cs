using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemReceiverCookAleatorio : MonoBehaviour, IDropHandler
{
    [Tooltip("Los 3 posibles IDs de ingrediente que puede tocar en esta partida.")]
    public List<string> acceptedItemIds;

    public PuzzleManager puzzleManager;
    public SFXPlayer sfxPlayer;

    public GameObject panelAntes;
    public GameObject panelDespues;

    public CookTracker cookTracker;

    public void OnDrop(PointerEventData eventData)
    {
        InventoryItemUI droppedItem = eventData.pointerDrag?.GetComponent<InventoryItemUI>();
        if (droppedItem != null && acceptedItemIds != null && acceptedItemIds.Contains(droppedItem.itemId))
        {
            Debug.Log("¡Ítem correcto usado!");
            sfxPlayer?.PlayBuble();
            Destroy(droppedItem.gameObject);

            puzzleManager?.ActivarPanelResuelto(panelAntes, panelDespues);
            cookTracker?.AddIngredient();
        }
        else
        {
            sfxPlayer?.PlayError();
            Debug.Log("Ítem incorrecto.");
        }
    }
}