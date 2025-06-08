using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

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
    private float duracionTransicion = 3f;

    private float tiempoTranscurrido = 0f;
    private bool partidaFinalizada = false;
    private bool animacion70Iniciada = false;
    private bool transicionColorHecha = false;

    private ColorAdjustments colorAdjustments;

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

        if (globalVolume != null && globalVolume.profile.TryGet(out colorAdjustments))
        {
            colorAdjustments.colorFilter.overrideState = true;
            colorAdjustments.colorFilter.value = colorInicial;
        }
    }

    void Update()
    {
        if (partidaFinalizada) return;

        tiempoTranscurrido += Time.deltaTime;

        float porcentaje = tiempoTranscurrido / duracionPartida;

        // Transición de color al 50%
        if (!transicionColorHecha && porcentaje >= 0.5f)
        {
            transicionColorHecha = true;
            StartCoroutine(TransicionarColor());
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
    }

    void IniciarAnimacion70()
    {
        animacion70Iniciada = true;

        if (animador70 != null)
        {
            animador70.SetTrigger(triggerAnimacion70);
            Debug.Log("Animación del 70% activada.");
        }
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
            {
                controller.canMove = false;
            }
        }
    }

    System.Collections.IEnumerator FadeInCanvasGroup()
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

    System.Collections.IEnumerator TransicionarColor()
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
}
