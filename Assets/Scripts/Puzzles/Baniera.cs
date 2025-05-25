using UnityEngine;
using System.Collections;

public class Baniera : MonoBehaviour
{
    public Animator animatorBañera;
    private bool yaActivado = false;
    public string itemNecesario = "Grifo";
    public CameraZoomEffect zoomEffect;
    public FaceCamera faceCameraScript;  // tu script a desactivar/reactivar
    Vector3 focusPoint = new Vector3(-10.75481f, 6.885497f, -5.326507f);
    Quaternion focusRotation = Quaternion.Euler(79.716f, -85.353f, 5.068f);

    public void Interactuar()
    {
        if (InventoryManager.Instance != null && InventoryManager.Instance.HasItem(itemNecesario))
        {
            zoomEffect.StartZoom(focusPoint, focusRotation, 1.5f, 2f);
            StartCoroutine(DisableFaceCameraTemporarily(3.5f));
            InventoryManager.Instance.RemoveItem(itemNecesario);
            animatorBañera.SetTrigger("Llenar");
            yaActivado = true;
            Debug.Log("Grifo usado. Animación de la bañera iniciada.");
        }
        else if (yaActivado)
        {
            zoomEffect.StartZoom(focusPoint, focusRotation, 1.5f, 2f);
            StartCoroutine(DisableFaceCameraTemporarily(3.5f));
        }
        else
        {
            Debug.Log("No tienes el objeto necesario (Grifo) para usar esto.");
        }
    }

    private IEnumerator DisableFaceCameraTemporarily(float duration)
    {
        if (faceCameraScript != null)
            faceCameraScript.enabled = false;

        yield return new WaitForSeconds(duration);

        if (faceCameraScript != null)
            faceCameraScript.enabled = true;
    }
}
