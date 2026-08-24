using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OllaReceiver : MonoBehaviour, IDropHandler
{
    [System.Serializable]
    public class IngredienteAceptado
    {
        public string itemId;

        [Tooltip("Deben ser EXACTAMENTE los mismos panelAntes/panelDespues que tiene la ItemReceiverCook dedicada de este ingrediente, para que se actualice igual aunque se suelte aquí en la olla en vez de en su ranura.")]
        public GameObject panelAntes;
        public GameObject panelDespues;
    }

    [Header("Ingredientes que esta olla acepta")]
    public List<IngredienteAceptado> ingredientesAceptados;

    [Header("Referencias")]
    public PuzzleManager puzzleManager;
    public SFXPlayer sfxPlayer;
    public CookTracker cookTracker;

    public void OnDrop(PointerEventData eventData)
    {
        InventoryItemUI droppedItem = eventData.pointerDrag?.GetComponent<InventoryItemUI>();

        if (droppedItem == null)
        {
            sfxPlayer?.PlayError();
            return;
        }

        IngredienteAceptado match = ingredientesAceptados?.Find(i => i.itemId == droppedItem.itemId);

        if (match != null)
        {
            Debug.Log("¡Ítem correcto usado en la olla!");
            sfxPlayer?.PlayBuble();
            Destroy(droppedItem.gameObject);

            // Replica el mismo cambio visual que haría la ranura dedicada de este ingrediente
            puzzleManager?.ActivarPanelResuelto(match.panelAntes, match.panelDespues);

            cookTracker?.AddIngredient();
        }
        else
        {
            sfxPlayer?.PlayError();
            Debug.Log("Ítem incorrecto en la olla.");
        }
    }
}