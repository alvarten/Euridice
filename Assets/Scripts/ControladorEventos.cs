using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;
using System.Collections.Generic;

public class ControladorEventos : MonoBehaviour
{
    [Header("Configuración del tiempo")]
    public float duracionPartida = 492f;

    [Header("Panel de final")]
    public GameObject panelFinal;
    public CanvasGroup canvasGroup;

    [Header("Animación del 70%")]
    public Animator animador70;
    public string triggerAnimacion70 = "Start70Animation";
    private DepthOfField depthOfField;
    public float dofStartNormal = 50f;
    public float dofStartAnimacion = 350f;

    [Header("Jugador")]
    public GameObject player;

    // ============================================================
    // POST-PROCESADO (Volume)
    // ============================================================
    [Header("Post-Procesado")]
    public Volume globalVolume;
    public float saturacionInicial = 0f;
    public float saturacionFinal = -60f;
    public float vignetteInicial = 0f;
    public float vignetteFinal = 0.4f;
    [Tooltip("Filtro de color aplicado sobre la imagen. Blanco = neutro, azulado = atmosfera fria/miedo")]
    public Color colorFiltroInicial = Color.white;
    public Color colorFiltroFinal = new Color(0.65f, 0.75f, 1f);

    // ============================================================
    // TRANSICION GENERAL DE ATMOSFERA (luces + post-proceso, sincronizados)
    // ============================================================
    [Header("Transición de Atmósfera de Miedo")]
    [Range(0f, 1f)] public float inicioTransicion = 0.5f; // % de la partida en que empieza a oscurecer
    [Tooltip("Curva de progreso 0-1. Por defecto ease-in-out para que el cambio no se note brusco al empezar/terminar.")]
    public AnimationCurve curvaTransicion = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    // ============================================================
    // LUZ PRINCIPAL (Directional Light)
    // ============================================================
    [Header("Luz Principal (Directional)")]
    public Light luzPrincipal;
    public float intensidadSolInicial = 1f;
    public float intensidadSolFinal = 0.05f;
    public Color colorSolInicial = new Color(1f, 0.95f, 0.8f);   // cálido
    public Color colorSolFinal = new Color(0.35f, 0.5f, 0.9f);   // azulado frío

    // ============================================================
    // LUZ AMBIENTE
    // ============================================================
    [Header("Luz Ambiente")]
    [Tooltip("Solo tiene efecto visible si en Lighting Settings > Environment, 'Environment Lighting Source' = Color.")]
    public Color ambienteColorInicial = new Color(1f, 0.95f, 0.85f);
    public Color ambienteColorFinal = new Color(0.15f, 0.22f, 0.4f);
    [Tooltip("Multiplicador de luz ambiente. Tiene efecto tanto si el source es Color como Skybox/Gradient.")]
    public float ambienteIntensidadInicial = 1f;
    public float ambienteIntensidadFinal = 0.15f;

    // ============================================================
    // LUCES DESTACADAS (se mantienen cálidas para resaltar zonas)
    // ============================================================
    [System.Serializable]
    public class LuzDestacada
    {
        public Light luz;
        public Color colorCalido = new Color(1f, 0.75f, 0.3f);
        public float intensidadBase = 2f;
        [Tooltip("Multiplicador aplicado a la intensidad al final de la transición. >1 hace que destaque más por contraste con el resto ya oscuro.")]
        public float multiplicadorIntensidadFinal = 1.3f;

        [Header("Parpadeo (opcional, tipo vela/farol)")]
        public bool parpadeo = false;
        [Range(0f, 1f)] public float parpadeoFuerza = 0.15f;
        public float parpadeoVelocidad = 3f;

        [HideInInspector] public float semillaParpadeo;
    }

    [Header("Luces Destacadas")]
    public List<LuzDestacada> lucesDestacadas = new List<LuzDestacada>();

    [Header("Sonido")]
    public SFXPlayer sfxPlayer;

    public float tiempoTranscurrido = 0f;
    private bool partidaFinalizada = false;
    private bool animacion70Iniciada = false;
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
    public PuzzleManager puzzleManager;


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
                colorAdjustments.colorFilter.overrideState = true;
                colorAdjustments.colorFilter.value = colorFiltroInicial;
            }

            if (globalVolume.profile.TryGet(out vignette))
            {
                vignette.intensity.overrideState = true;
                vignette.intensity.value = vignetteInicial;
            }

            if (globalVolume.profile.TryGet(out depthOfField))
            {
                depthOfField.gaussianStart.overrideState = true;
                depthOfField.gaussianEnd.overrideState = true;
                depthOfField.gaussianStart.value = 50f;
                depthOfField.gaussianEnd.value = 100f;
            }
        }

        if (luzPrincipal != null)
        {
            luzPrincipal.intensity = intensidadSolInicial;
            luzPrincipal.color = colorSolInicial;
        }

        RenderSettings.ambientLight = ambienteColorInicial;
        RenderSettings.ambientIntensity = ambienteIntensidadInicial;

        // Inicializar luces destacadas: color cálido fijo + semilla de parpadeo aleatoria
        // (la semilla evita que todas las luces parpadeen exactamente igual y a la vez)
        foreach (var ld in lucesDestacadas)
        {
            if (ld.luz == null) continue;
            ld.luz.color = ld.colorCalido;
            ld.luz.intensity = ld.intensidadBase;
            ld.semillaParpadeo = Random.Range(0f, 100f);
        }
    }

    void Update()
    {
        if (partidaFinalizada) return;

        tiempoTranscurrido += Time.deltaTime;

        float porcentaje = tiempoTranscurrido / duracionPartida;

        // Animación al 70%
        if (!animacion70Iniciada && porcentaje >= 0.7f)
        {
            IniciarAnimacion70();
        }

        // Activar guardián al cruzar el inicio de la transición de atmósfera
        if (!guardianActivado && porcentaje >= inicioTransicion)
        {
            ActivarGuardian();
        }

        // Transición de atmósfera (post-proceso + luces), calculada directamente
        // sobre tiempoTranscurrido cada frame, sin coroutines ni timers propios.
        ActualizarAtmosfera(porcentaje);

        // Final de partida
        if (tiempoTranscurrido >= duracionPartida)
        {
            FinalizarPartida();
        }
    }

    void ActualizarAtmosfera(float porcentaje)
    {
        float progresoLineal = 0f;
        if (porcentaje >= inicioTransicion)
        {
            progresoLineal = Mathf.Clamp01((porcentaje - inicioTransicion) / (1f - inicioTransicion));
        }

        float t = curvaTransicion.Evaluate(progresoLineal);

        // --- Post-procesado ---
        if (colorAdjustments != null)
        {
            colorAdjustments.saturation.value = Mathf.Lerp(saturacionInicial, saturacionFinal, t);
            colorAdjustments.colorFilter.value = Color.Lerp(colorFiltroInicial, colorFiltroFinal, t);
        }
        if (vignette != null)
        {
            vignette.intensity.value = Mathf.Lerp(vignetteInicial, vignetteFinal, t);
        }

        // --- Luz principal ---
        if (luzPrincipal != null)
        {
            luzPrincipal.intensity = Mathf.Lerp(intensidadSolInicial, intensidadSolFinal, t);
            luzPrincipal.color = Color.Lerp(colorSolInicial, colorSolFinal, t);
        }

        // --- Luz ambiente ---
        RenderSettings.ambientLight = Color.Lerp(ambienteColorInicial, ambienteColorFinal, t);
        RenderSettings.ambientIntensity = Mathf.Lerp(ambienteIntensidadInicial, ambienteIntensidadFinal, t);

        // --- Luces destacadas (se mantienen cálidas, opcionalmente se intensifican y/o parpadean) ---
        foreach (var ld in lucesDestacadas)
        {
            if (ld.luz == null) continue;

            float intensidadObjetivo = Mathf.Lerp(ld.intensidadBase, ld.intensidadBase * ld.multiplicadorIntensidadFinal, t);

            if (ld.parpadeo)
            {
                float ruido = Mathf.PerlinNoise(Time.time * ld.parpadeoVelocidad + ld.semillaParpadeo, 0f); // 0..1
                float factorParpadeo = 1f + (ruido - 0.5f) * 2f * ld.parpadeoFuerza;
                intensidadObjetivo *= factorParpadeo;
            }

            ld.luz.intensity = intensidadObjetivo;
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
            puzzleManager.CerrarPanelesParaAnimacion();
            animador70.SetTrigger(triggerAnimacion70);
            Debug.Log("Animación del 70% activada.");
        }

        if (zoomEffect != null)
        {
            zoomEffect.StartZoom(puntoZoom, rotacionZoom, duracionZoom, tiempoMantenerZoom);
        }

        StartCoroutine(PicoDepthOfField(duracionZoom + tiempoMantenerZoom));
        StartCoroutine(DisableFaceCameraTemporarily(duracionZoom + tiempoMantenerZoom));
    }
    IEnumerator PicoDepthOfField(float duracion)
    {
        if (depthOfField == null) yield break;

        depthOfField.gaussianStart.value = dofStartAnimacion;

        yield return new WaitForSeconds(duracion);

        depthOfField.gaussianStart.value = dofStartNormal;
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

    private IEnumerator DisableFaceCameraTemporarily(float duration)
    {
        if (faceCameraScript != null)
            faceCameraScript.enabled = false;

        yield return new WaitForSeconds(duration);

        if (faceCameraScript != null)
            faceCameraScript.enabled = true;
    }
}