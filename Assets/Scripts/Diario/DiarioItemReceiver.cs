using UnityEngine;
using UnityEngine.EventSystems;

public class DiarioItemReceiver : MonoBehaviour, IDropHandler
{
    public string acceptedItemId;
    public SFXPlayer sfxPlayer;

    [Header("Para modificar el diario")]
    public LibroDiario libroDiario;
    public Sprite nuevoSpritePagina3;

    public void OnDrop(PointerEventData eventData)
    {
        InventoryItemUI droppedItem = eventData.pointerDrag?.GetComponent<InventoryItemUI>();
        if (droppedItem != null && droppedItem.itemId == acceptedItemId)
        {
            Debug.Log("¡Ítem correcto usado!");
            sfxPlayer.PlayPage();
            Destroy(droppedItem.gameObject);

            // Activar cambio en el diario
            if (libroDiario != null && nuevoSpritePagina3 != null)
            {
                libroDiario.CambiarSpritePagina3(nuevoSpritePagina3);
            }
        }
        else
        {
            Debug.Log("Ítem incorrecto.");
        }
    }
}
