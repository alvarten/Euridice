using UnityEngine;
using UnityEngine.EventSystems;

public class ItemReceiverWithTogglePanel : MonoBehaviour, IDropHandler
{
    public string acceptedItemId;
    public SFXPlayer sfxPlayer;

    [Header("Interfaz de usuario")]
    public GameObject panelParaActivar; // Este panel será toggled (activado/desactivado)

    [Header("Cambio de interactuables")]
    public GameObject interactuableAntes;   // El objeto que se desactivará
    public GameObject interactuableDespues; // El objeto que se activará

    [Header("Puzzle Manager")]
    public PuzzleManager puzzleManager; // Referencia al PuzzleManager que tiene TogglePuzzlePanel()

    public void OnDrop(PointerEventData eventData)
    {
        InventoryItemUI droppedItem = eventData.pointerDrag?.GetComponent<InventoryItemUI>();
        if (droppedItem != null && droppedItem.itemId == acceptedItemId)
        {
            Debug.Log("¡Ítem correcto usado!");
            //sfxPlayer.PlayChest();
            Destroy(droppedItem.gameObject);

            // Activar el panel usando PuzzleManager
            if (puzzleManager != null && panelParaActivar != null)
            {
                puzzleManager.TogglePuzzlePanel(panelParaActivar);
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
