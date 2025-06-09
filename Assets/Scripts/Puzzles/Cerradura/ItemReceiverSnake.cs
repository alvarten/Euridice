using UnityEngine;
using UnityEngine.EventSystems;

public class ItemReceiverSnake : MonoBehaviour, IDropHandler
{
    public string acceptedItemId;
    public SFXPlayer sfxPlayer;

    [Header("Interfaz de usuario")]

    public GameObject panelParaActivar; // Este panel será toggled (activado)
    public GameObject panelParaDesactivar; // Este panel será toggled (desactivado)

    [Header("Cambio de interactuables")]
    public GameObject interactuableAntes;   // El objeto que se desactivará
    public GameObject interactuableDespues; // El objeto que se activará

    [Header("Puzzle Manager")]
    public PuzzleManager puzzleManager;

    public void OnDrop(PointerEventData eventData)
    {
        InventoryItemUI droppedItem = eventData.pointerDrag?.GetComponent<InventoryItemUI>();
        if (droppedItem != null && droppedItem.itemId == acceptedItemId)
        {
            Debug.Log("¡Ítem correcto usado!");
            //sfxPlayer.PlayChest(); //Sonido al abrir la cerradura
            Destroy(droppedItem.gameObject);

            // Activar el panel usando PuzzleManager
            if (puzzleManager != null && panelParaActivar != null && panelParaDesactivar != null)
            {
                puzzleManager.TogglePuzzlePanel(panelParaActivar);
                puzzleManager.TogglePuzzlePanel(panelParaDesactivar);
            }

            // Cambiar interactuables
            if (interactuableAntes != null)
                interactuableAntes.SetActive(false);

            if (interactuableDespues != null)
                interactuableDespues.SetActive(true);
        }
        else
        {
            Debug.Log("Ítem incorrecto.");
        }
    }
}
