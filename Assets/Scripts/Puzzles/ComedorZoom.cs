using UnityEngine;
using System.Collections;
using TMPro;

public class ComedorZoom : MonoBehaviour
{
    [Header("Referencias")]
    public CameraZoomEffect zoomEffect;
    public FaceCamera faceCameraScript;

    [Header("Configuración del Zoom")]
    public Vector3 focusPoint = new Vector3(0, 5, 0); 
    public Vector3 eulerRotation = new Vector3(60, 0, 0); 
    public float zoomDuration = 1.5f;
    public float zoomHoldTime = 2f;

    [Header("Objeto a desactivar durante el zoom")]
    public GameObject objetoInteractuable;

    [Header("Opcional: Restaurar Cámara")]
    public bool restoreAfterDelay = false;
    public float restoreDelay = 3.5f;

    [Header("Texto a mostrar")]
    public TextMeshProUGUI uiText;
    public string message = "Texto de ejemplo";
    private CanvasGroup canvasGroup;

    [Header("Duraciones")]
    public float fadeInDuration = 1f;
    public float displayDuration = 2f;
    public float fadeOutDuration = 1f;

    private void Awake() //nuevo
    {
        if (uiText == null)
        {
            Debug.LogError("No se asignó ningún Text UI.");
            return;
        }

        canvasGroup = uiText.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = uiText.gameObject.AddComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;
    }

    public void ActivarZoom()
    {
        Quaternion focusRotation = Quaternion.Euler(eulerRotation);

        if (zoomEffect != null) 
        {
            zoomEffect.StartZoomUntilKey(focusPoint, focusRotation, zoomDuration, KeyCode.E, objetoInteractuable);
        }
        ShowMessage();

    }

    private IEnumerator DisableFaceCameraTemporarily(float duration)
    {
        if (faceCameraScript != null)
            faceCameraScript.enabled = false;

        yield return new WaitForSeconds(duration);

        if (faceCameraScript != null)
            faceCameraScript.enabled = true;
    }

    private IEnumerator RestoreOrbitalAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (zoomEffect != null)
            zoomEffect.RestoreOrbitalCamera();
    }

    //Metodo para mostrar el mensaje
    public void ShowMessage()
    {
        uiText.text = message;
        StartCoroutine(ShowAndActivate());
    }

    private IEnumerator ShowAndActivate()
    {
        // Fade In
        yield return StartCoroutine(FadeTo(1f, fadeInDuration));

        // Mantener texto visible
        yield return new WaitForSeconds(displayDuration);

        // Fade Out
        yield return StartCoroutine(FadeTo(0f, fadeOutDuration));

    }

    private IEnumerator FadeTo(float targetAlpha, float duration)
    {
        float startAlpha = canvasGroup.alpha;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
    }
}
