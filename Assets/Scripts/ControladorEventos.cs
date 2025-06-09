using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class ControladorEventos : MonoBehaviour
{
    [Header("Configuración del tiempo")]
    public float duracionPartida = 600f;

    [Header("Panel de final")]
    public GameObject panelFinal;
    public CanvasGroup canvasGroup;

    [Header("Animación del 70%")]
    public Animator animador70;
    public string triggerAnimacion70 = "Start70Animation";

    [Header("Jugador")]
    public GameObject player;

    [Header("Post-Procesado")]
    public Volume globalVolume;
    public Color colorInicial = Color.white;
    public Color colorFinal = Color.red;
    public float saturacionInicial = 0f;
    public float saturacionFinal = -100f;
    public float vignetteInicial = 0f;
    public float vignetteFinal = 0.4f;

    [Header("Sonido")]
    public SFXPlayer sfxPlayer;

    public float tiempoTranscurrido = 0f;
    private bool partidaFinalizada = false;
    private bool animacion70Iniciada = false;
    private bool transicionPostFXHecha = false;
    private bool transicionColorHecha = false;
    private bool guardianActivado = false;

    private ColorAdjustments colorAdjustments;
    private Vignette vignette;

    [Header("Zoom de Cámara")]
    public CameraZoomEffect zoomEffect;
    public Vector3 puntoZoom = new Vector3(-10.75481f, 6.885497f, -5.326507f);
    public Quaternion rotacionZoom = Quaternion.Euler(79.716f, -85.353f, 5.068f);
    public float duracionZoom = 1.5f;
    public float tiempoMantenerZoom = 2f;
    public FaceCamera faceCameraScript;


    void Start()
    {
        if (panelFinal != null)
            panelFinal.SetActive(false);

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        if (globalVolume != null)
        {
            if (globalVolume.profile.TryGet(out colorAdjustments))
            {
                colorAdjustments.saturation.overrideState = true;
                colorAdjustments.saturation.value = saturacionInicial;
            }

            if (globalVolume.profile.TryGet(out vignette))
            {
                vignette.intensity.overrideState = true;
                vignette.intensity.value = vignetteInicial;
            }
        }
    }

    void Update()
    {
        if (partidaFinalizada) return;

        tiempoTranscurrido += Time.deltaTime;

        float porcentaje = tiempoTranscurrido / duracionPartida;

        // Transición de post-procesado al 50%
        if (!transicionPostFXHecha && porcentaje >= 0.5f)
        {
            transicionPostFXHecha = true;
            StartCoroutine(TransicionarPostProcesado());

            // Activar guardián
            ActivarGuardian();
        }

        // Animación al 70%
        if (!animacion70Iniciada && porcentaje >= 0.7f)
        {
            IniciarAnimacion70();
        }

        // Final de partida
        if (tiempoTranscurrido >= duracionPartida)
        {
            FinalizarPartida();
        }

        if (!transicionColorHecha && porcentaje >= 0.5f)
        {
            transicionColorHecha = true;
            StartCoroutine(TransicionarColor());
        }
    }

    void ActivarGuardian()
    {
        if (guardianActivado || sfxPlayer == null) return;

        guardianActivado = true;
        sfxPlayer.PlayIntroGuardian();
        StartCoroutine(RisasAleatorias());
    }

    IEnumerator RisasAleatorias()
    {
        while (!partidaFinalizada)
        {
            float porcentaje = tiempoTranscurrido / duracionPartida;
            float delay;

            if (porcentaje >= 0.5f && porcentaje < 0.65f)
            {
                delay = 30f;
            }
            else if (porcentaje >= 0.75f && porcentaje < 0.95f)
            {
                delay = 20f;
            }
            else
            {
                yield return new WaitForSeconds(5f);
                continue;
            }

            yield return new WaitForSeconds(delay);

            porcentaje = tiempoTranscurrido / duracionPartida;

            if ((porcentaje >= 0.5f && porcentaje < 0.65f) || (porcentaje >= 0.75f && porcentaje < 0.95f))
            {
                if (Random.value < 0.5f)
                {
                    sfxPlayer.PlayLaugh();
                }
            }
        }
    }


    void IniciarAnimacion70()
    {
        animacion70Iniciada = true;

        if (animador70 != null)
        {
            sfxPlayer.PlayChoke();
            animador70.SetTrigger(triggerAnimacion70);
            Debug.Log("Animación del 70% activada.");
        }
        
        if (zoomEffect != null)
        {
            zoomEffect.StartZoom(puntoZoom, rotacionZoom, duracionZoom, tiempoMantenerZoom);
        }        
        StartCoroutine(DisableFaceCameraTemporarily(duracionZoom + tiempoMantenerZoom));
    }

    void FinalizarPartida()
    {
        partidaFinalizada = true;

        if (panelFinal != null)
            panelFinal.SetActive(true);

        if (canvasGroup != null)
            StartCoroutine(FadeInCanvasGroup());

        if (player != null)
        {
            var controller = player.GetComponent<PlayerController>();
            if (controller != null)
                controller.canMove = false;
        }
    }

    IEnumerator FadeInCanvasGroup()
    {
        float duracion = 2f;
        float t = 0f;

        while (t < duracion)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t / duracion);
            yield return null;
        }

        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    IEnumerator TransicionarPostProcesado()
    {
        float t = 0f;
        float duracionTransicion = duracionPartida / 2f;

        while (t < duracionTransicion)
        {
            t += Time.deltaTime;
            float progreso = t / duracionTransicion;

            if (colorAdjustments != null)
                colorAdjustments.saturation.value = Mathf.Lerp(saturacionInicial, saturacionFinal, progreso);

            if (vignette != null)
                vignette.intensity.value = Mathf.Lerp(vignetteInicial, vignetteFinal, progreso);

            yield return null;
        }

        if (colorAdjustments != null)
            colorAdjustments.saturation.value = saturacionFinal;

        if (vignette != null)
            vignette.intensity.value = vignetteFinal;
    }

    IEnumerator TransicionarColor()
    {
        float t = 0f;
        float duracionTransicion = duracionPartida / 2f;

        while (t < duracionTransicion)
        {
            t += Time.deltaTime;
            Color nuevoColor = Color.Lerp(colorInicial, colorFinal, t / duracionTransicion);

            if (colorAdjustments != null)
                colorAdjustments.colorFilter.value = nuevoColor;

            yield return null;
        }

        if (colorAdjustments != null)
            colorAdjustments.colorFilter.value = colorFinal;
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
