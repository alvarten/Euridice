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

    [Header("Objeto a desactivar durante el zoom")]
    public GameObject objetoInteractuable;

    //---
    [Header("Texto a mostrar")]
    public TextMeshProUGUI uiText;
    public string message = "Texto de ejemplo";
    private CanvasGroup textCanvasGroup;

    [Header("Indicador de 'Pulsa E para volver'")]
    [Tooltip("Aparece siempre que se hace zoom (con item, sin item o ya activado), y desaparece justo cuando el jugador pulsa E.")]
    public GameObject panelIndicadorE;
    private CanvasGroup indicadorCanvasGroup;

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
        }
        else
        {
            textCanvasGroup = uiText.GetComponent<CanvasGroup>();
            if (textCanvasGroup == null)
                textCanvasGroup = uiText.gameObject.AddComponent<CanvasGroup>();
            textCanvasGroup.alpha = 0f;
        }

        if (panelIndicadorE != null)
        {
            indicadorCanvasGroup = panelIndicadorE.GetComponent<CanvasGroup>();
            if (indicadorCanvasGroup == null)
                indicadorCanvasGroup = panelIndicadorE.AddComponent<CanvasGroup>();
            indicadorCanvasGroup.alpha = 0f;
        }
    }

    private void OnEnable()
    {
        if (zoomEffect != null)
            zoomEffect.OnZoomEndedByKey += OcultarIndicadorE;
    }

    private void OnDisable()
    {
        if (zoomEffect != null)
            zoomEffect.OnZoomEndedByKey -= OcultarIndicadorE;
    }

    public void Interactuar()
    {
        if (InventoryManager.Instance != null && InventoryManager.Instance.HasItem(itemNecesario))
        {
            IniciarZoom();
            //StartCoroutine(DisableFaceCameraTemporarily(3.5f));
            InventoryManager.Instance.RemoveItem(itemNecesario);
            animatorBañera.SetTrigger("Llenar");
            yaActivado = true;
            Debug.Log("Grifo usado. Animación de la bañera iniciada.");
            sfxPlayer.PlayWater();
        }
        else if (yaActivado)
        {
            IniciarZoom();
            //StartCoroutine(DisableFaceCameraTemporarily(3.5f));
        }
        else
        {
            IniciarZoom();
            Debug.Log("No tienes el objeto necesario (Grifo) para usar esto.");
            ShowMessage(); //Mensaje de no tener el objeto necesario
        }
    }

    // Centraliza el arranque del zoom para que el indicador de E aparezca
    // siempre, sin tener que repetir la llamada en las tres ramas de arriba.
    private void IniciarZoom()
    {
        zoomEffect.StartZoomUntilKey(focusPoint, focusRotation, 1.5f, KeyCode.E, objetoInteractuable);
        MostrarIndicadorE();
    }

    //Metodo para mostrar el mensaje
    public void ShowMessage()
    {
        if (uiText == null || textCanvasGroup == null) return;

        uiText.text = message;
        StartCoroutine(ShowAndActivate());
    }

    private IEnumerator ShowAndActivate()
    {
        // Fade In
        yield return StartCoroutine(FadeCanvasGroupTo(textCanvasGroup, 1f, fadeInDuration));
        // Mantener texto visible
        yield return new WaitForSeconds(displayDuration);
        // Fade Out
        yield return StartCoroutine(FadeCanvasGroupTo(textCanvasGroup, 0f, fadeOutDuration));
    }

    // El indicador de E hace fade in al iniciar el zoom, y NO se oculta con un
    // temporizador: lo oculta OcultarIndicadorE(), llamado por el evento
    // OnZoomEndedByKey de CameraZoomEffect cuando el jugador pulsa la tecla.
    private void MostrarIndicadorE()
    {
        if (indicadorCanvasGroup == null) return;
        StartCoroutine(FadeCanvasGroupTo(indicadorCanvasGroup, 1f, fadeInDuration));
    }

    private void OcultarIndicadorE()
    {
        if (indicadorCanvasGroup == null) return;
        StartCoroutine(FadeCanvasGroupTo(indicadorCanvasGroup, 0f, fadeOutDuration));
    }

    private IEnumerator FadeCanvasGroupTo(CanvasGroup cg, float targetAlpha, float duration)
    {
        float startAlpha = cg.alpha;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cg.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);
            yield return null;
        }
        cg.alpha = targetAlpha;
    }
}