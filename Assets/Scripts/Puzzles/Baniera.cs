using UnityEngine;

public class Baniera : MonoBehaviour
{
    public Animator animatorBañera;
    private bool yaActivado = false;
    public string itemNecesario = "Grifo";
    public CameraZoomEffect zoomEffect;
    Vector3 focusPoint = new Vector3(-10.82611f, 5.560f, -3.807848f); // tu punto elegido
    Quaternion focusRotation = Quaternion.Euler(72.055f, -90.336f, 0f); // o la rotación de la cámara en escena

    public void Interactuar()
    {
        //if (yaActivado) return; //Por si queremos que se active una sola vez

        if (InventoryManager.Instance != null && InventoryManager.Instance.HasItem(itemNecesario))
        {
            zoomEffect.StartZoom(focusPoint, focusRotation);
            InventoryManager.Instance.RemoveItem(itemNecesario);
            animatorBañera.SetTrigger("Llenar");
            yaActivado = true;
            Debug.Log("Grifo usado. Animación de la bañera iniciada.");
        }
        else if (yaActivado)
        {
            zoomEffect.StartZoom(focusPoint, focusRotation);
        }
        else
        {
            Debug.Log("No tienes el objeto necesario (Grifo) para usar esto.");
        }
    }
}
