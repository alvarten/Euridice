using UnityEngine;
using System.Collections;
using TMPro;

public class Baniera : MonoBehaviour
{
    public SFXPlayer sfxPlayer;

    [Header("Animacion")]
    public Animator animatorBañera;
    private bool yaActivado = false;
    public string itemNecesario = "Grifo";
    public CameraZoomEffect zoomEffect;
    public FaceCamera faceCameraScript;
    Vector3 focusPoint = new Vector3(-10.75481f, 6.885497f, -5.326507f);
    Quaternion focusRotation = Quaternion.Euler(79.716f, -85.353f, 5.068f);

    //---
    [Header("Texto a mostrar")]
    public TextMeshProUGUI uiText;
    public string message = "Texto de ejemplo";
    private CanvasGroup canvasGroup;

    [Header("Duraciones")]
    public float fadeInDuration = 1f;
    public float displayDuration = 2f;
    public float fadeOutDuration = 1f;
    //---
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

            sfxPlayer.PlayWater();
        }
        else if (yaActivado)
        {
            zoomEffect.StartZoom(focusPoint, focusRotation, 1.5f, 2f);
            StartCoroutine(DisableFaceCameraTemporarily(3.5f));
        }
        else
        {
            Debug.Log("No tienes el objeto necesario (Grifo) para usar esto.");
            ShowMessage(); //nuevo

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

    //----- nuevo
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
