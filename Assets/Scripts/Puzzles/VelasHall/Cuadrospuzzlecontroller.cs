using System.Collections;
using UnityEngine;

// Va en un GameObject "gestor" de la escena (puede ser el mismo que tiene VelasManager, o uno nuevo).
// Se conecta desde el Interactuable de la zona de aproximación: arrastra este objeto y
// selecciona CuadrosPuzzleController > IniciarZoomCuadros en el UnityEvent onInteract.
public class CuadrosPuzzleController : MonoBehaviour
{
    [Header("Zoom")]
    public CameraZoomEffect zoomEffect;
    public Vector3 focusPoint;
    public Vector3 eulerRotation;
    public float zoomDuration = 1.5f;

    [Header("Objeto a desactivar durante el zoom (icono 'E' de la zona)")]
    public GameObject objetoInteractuable;

    [Header("Cuadros clicables (mismo orden que uses en VelasManager, aunque no es obligatorio)")]
    public CuadroClickable[] cuadros;

    [Header("Indicador de 'Pulsa E para volver'")]
    [Tooltip("Aparece al iniciar el zoom y desaparece justo cuando el jugador pulsa E (o cuando se fuerza la salida al completar el puzle).")]
    public GameObject panelIndicadorE;
    public float fadeInDuration = 1f;
    public float fadeOutDuration = 1f;
    private CanvasGroup indicadorCanvasGroup;

    private void Awake()
    {
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
            zoomEffect.OnZoomEndedByKey += OnZoomTerminado;
    }

    private void OnDisable()
    {
        if (zoomEffect != null)
            zoomEffect.OnZoomEndedByKey -= OnZoomTerminado;
    }

    // Llamado desde el Interactuable de la zona de aproximación (onInteract)
    public void IniciarZoomCuadros()
    {
        if (zoomEffect == null) return;

        Quaternion rot = Quaternion.Euler(eulerRotation);
        zoomEffect.StartZoomUntilKey(focusPoint, rot, zoomDuration, KeyCode.E, objetoInteractuable);

        MostrarIndicadorE();

        // Esperamos a que la cámara termine de moverse antes de permitir clicks,
        // así no se puede seleccionar un cuadro a mitad de la transición.
        StartCoroutine(ActivarClicksTrasTransicion());
    }

    private IEnumerator ActivarClicksTrasTransicion()
    {
        yield return new WaitForSeconds(zoomDuration);
        SetClicksCuadros(true);
    }

    // Se llama tanto si el jugador pulsa E como si se fuerza la salida (VelasManager.ForzarSalidaZoom)
    private void OnZoomTerminado()
    {
        SetClicksCuadros(false);
        OcultarIndicadorE();
    }

    private void SetClicksCuadros(bool habilitado)
    {
        if (cuadros == null) return;
        foreach (var cuadro in cuadros)
        {
            if (cuadro != null)
                cuadro.enabled = habilitado;
        }
    }

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