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
    private CanvasGroup textCanvasGroup;

    [Header("Indicador de 'Pulsa E para volver'")]
    [Tooltip("Panel de UI (con o sin CanvasGroup ya puesto) que se muestra mientras dura el zoom. Aparece a la vez que el texto y desaparece justo cuando el jugador pulsa E, no con un temporizador.")]
    public GameObject panelIndicadorE;
    private CanvasGroup indicadorCanvasGroup;

    [Header("Duraciones")]
    public float fadeInDuration = 1f;
    public float displayDuration = 2f;
    public float fadeOutDuration = 1f;

    private void Awake()
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

    public void ActivarZoom()
    {
        Quaternion focusRotation = Quaternion.Euler(eulerRotation);
        if (zoomEffect != null)
        {
            zoomEffect.StartZoomUntilKey(focusPoint, focusRotation, zoomDuration, KeyCode.E, objetoInteractuable);
        }

        ShowMessage();
        MostrarIndicadorE();
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

    // Método para mostrar el mensaje (fade in -> espera -> fade out, temporizado como siempre)
    public void ShowMessage()
    {
        if (uiText == null || textCanvasGroup == null) return;

        uiText.text = message;
        StartCoroutine(ShowAndActivate());
    }

    private IEnumerator ShowAndActivate()
    {
        yield return StartCoroutine(FadeCanvasGroupTo(textCanvasGroup, 1f, fadeInDuration));
        yield return new WaitForSeconds(displayDuration);
        yield return StartCoroutine(FadeCanvasGroupTo(textCanvasGroup, 0f, fadeOutDuration));
    }

    // El indicador de E solo hace fade in con el mismo ritmo que el texto.
    // Su fade out NO está temporizado: lo dispara OcultarIndicadorE() cuando
    // CameraZoomEffect avisa de que el jugador ha pulsado la tecla de salida.
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

    // Coroutine de fade genérica, reutilizada tanto por el texto como por el indicador de E
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