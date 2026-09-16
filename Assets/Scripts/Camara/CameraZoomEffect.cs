using UnityEngine;
using System;
using System.Collections;

public class CameraZoomEffect : MonoBehaviour
{
    [Header("Valores por defecto")]
    public float zoomDuration = 1.5f;
    public float holdDuration = 2f;

    [Header("Componentes externos")]
    public OrbitalCamera orbitalCamera;
    public MonoBehaviour faceCameraScript;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private Coroutine zoomCoroutine;
    public bool isZooming = false;

    public GameObject player;

    private Collider playerCollider;
    private PlayerController playerController;

    // Se dispara justo en el momento en que el jugador pulsa la tecla de salida
    // en StartZoomUntilKey (antes de que la cámara empiece a volver a su posición original),
    // o cuando un zoom nuevo cancela a la fuerza uno anterior que estaba esperando tecla.
    public event Action OnZoomEndedByKey;

    private void Awake()
    {
        if (player != null)
        {
            playerCollider = player.GetComponent<Collider>();
            playerController = player.GetComponent<PlayerController>();
        }
    }

    private bool forzarSalidaZoom = false;

    // Guarda si orbitalCamera estaba activada justo antes de empezar un zoom, para
    // restaurar ese mismo estado al terminar (en vez de forzar siempre "activada").
    // Esto es lo que permite respetar el modo de encuadre fijo de SalaTrigger.
    private bool orbitalHabilitadoAntesDelZoom = true;

    // --- Estado auxiliar para poder deshacer correctamente un ZoomUntilKey
    // aunque se corte a mitad por culpa de un zoom nuevo con prioridad ---
    private GameObject objetoDesactivadoActual = null;
    private bool esperandoTecla = false;

    // True mientras dura un zoom "prioritario" (StartZoom, p.ej. el del evento del 70%).
    // Mientras esté activo, no se puede iniciar ningún StartZoomUntilKey ni leer la
    // tecla de salida de otros, de forma que el jugador no pueda activar zooms de
    // interacción (como los de ComedorZoom) durante el evento.
    // Es estático (y no depende de "canMove") porque canMove también se pone a false
    // durante puzles o interacciones normales, y NO queremos bloquear la tecla E en
    // esos casos (p.ej. para poder salir de un puzle con E) — solo durante el evento.
    private static bool zoomPrioritarioActivo = false;
    public static bool EventoActivo => zoomPrioritarioActivo;
    public bool IsEventoActivo => zoomPrioritarioActivo;

    // Permite terminar StartZoomUntilKey desde fuera (p.ej. al completar un minijuego),
    // sin esperar a que el jugador pulse físicamente la tecla de salida.
    public void ForzarSalidaZoom()
    {
        forzarSalidaZoom = true;
    }

    // Deja la cámara y todo lo que un zoom pueda haber tocado (movimiento del
    // jugador, faceCamera, objeto desactivado, orbital) en un estado consistente,
    // y corta cualquier coroutine de zoom en marcha. Se llama SIEMPRE antes de
    // empezar un zoom nuevo, para que el nuevo zoom parta de una base limpia
    // y no se queden "cleanups" pendientes de un zoom anterior interrumpido.
    private void ForzarResetZoom()
    {
        if (zoomCoroutine != null)
        {
            StopCoroutine(zoomCoroutine);
            zoomCoroutine = null;
        }

        if (esperandoTecla)
        {
            // Avisamos igualmente a quien escuche (p.ej. para ocultar el "Pulsa E"),
            // aunque la salida no haya sido por la tecla sino por un zoom prioritario.
            OnZoomEndedByKey?.Invoke();
            esperandoTecla = false;
        }

        if (objetoDesactivadoActual != null)
        {
            objetoDesactivadoActual.SetActive(true);
            objetoDesactivadoActual = null;
        }

        if (faceCameraScript != null)
            faceCameraScript.enabled = true;

        TogglePlayerMovement(true);
        ToggleHitboxYGravedad(true);

        if (orbitalCamera != null)
            orbitalCamera.enabled = orbitalHabilitadoAntesDelZoom;

        isZooming = false;
        zoomPrioritarioActivo = false;
    }

    // Mueve la cámara a una posición y rotación específicas con transición suave.
    public void StartZoom(Vector3 targetPosition, Quaternion targetRotation, float transitionDuration, float holdTime)
    {
        ForzarResetZoom();

        zoomCoroutine = StartCoroutine(ZoomSequence(targetPosition, targetRotation, transitionDuration, holdTime));
    }

    public void StartZoomUntilKey(Vector3 targetPosition, Quaternion targetRotation, float transitionDuration, KeyCode exitKey, GameObject objectToDisable = null)
    {
        // Si hay un zoom prioritario (evento) en marcha, ignoramos cualquier intento
        // de arrancar un zoom de interacción: el jugador no puede "colarse" con E
        // mientras el evento está ocurriendo.
        if (zoomPrioritarioActivo)
            return;

        ForzarResetZoom();

        zoomCoroutine = StartCoroutine(ZoomUntilKeySequence(targetPosition, targetRotation, transitionDuration, exitKey, objectToDisable));
    }

    public void CancelZoom()
    {
        if (zoomCoroutine != null)
        {
            bool estabaEsperandoTecla = esperandoTecla;

            ForzarResetZoom();

            // Transición suave de vuelta a la posición original (en vez del corte
            // instantáneo que ya hace ForzarResetZoom sobre movimiento/orbital/etc).
            zoomCoroutine = StartCoroutine(SmoothTransition(originalPosition, originalRotation, zoomDuration));
        }
    }

    public void RestoreOrbitalCamera()
    {
        if (orbitalCamera != null)
            orbitalCamera.enabled = true;
    }


    // Mueve la cámara a una posición y rotación específicas con transición suave de manera indefinida.
    public void SetCameraToPositionSmooth(Vector3 targetPosition, Quaternion targetRotation, float duration)
    {
        ForzarResetZoom();

        zoomCoroutine = StartCoroutine(PermanentTransition(targetPosition, targetRotation, duration));
    }

    IEnumerator ZoomSequence(Vector3 targetPosition, Quaternion targetRotation, float transitionDuration, float holdTime)
    {
        isZooming = true;
        zoomPrioritarioActivo = true;

        originalPosition = transform.position;
        originalRotation = transform.rotation;

        orbitalHabilitadoAntesDelZoom = orbitalCamera != null && orbitalCamera.enabled;
        if (orbitalCamera != null)
            orbitalCamera.enabled = false;

        // Bloqueamos al jugador (movimiento e interacción) mientras dura este zoom,
        // igual que ya se hacía en ZoomUntilKeySequence. Sin esto, el jugador podía
        // moverse y activar otro zoom (p.ej. un ComedorZoom) mientras el zoom del
        // evento estaba en marcha, dejando la cámara en un estado inconsistente.
        TogglePlayerMovement(false);

        // Además desactivamos la hitbox (para que no pueda colisionar con ningún
        // trigger de interacción aunque esté quieto encima de uno) y la gravedad
        // (para que no se caiga o atraviese el suelo al no tener collider).
        ToggleHitboxYGravedad(false);

        yield return StartCoroutine(SmoothTransition(targetPosition, targetRotation, transitionDuration));

        yield return new WaitForSeconds(holdTime);

        yield return StartCoroutine(SmoothTransition(originalPosition, originalRotation, transitionDuration));

        TogglePlayerMovement(true);
        ToggleHitboxYGravedad(true);

        if (orbitalCamera != null)
            orbitalCamera.enabled = orbitalHabilitadoAntesDelZoom;

        isZooming = false;
        zoomPrioritarioActivo = false;
        zoomCoroutine = null;
    }

    IEnumerator ZoomUntilKeySequence(Vector3 targetPosition, Quaternion targetRotation, float transitionDuration, KeyCode exitKey, GameObject objectToDisable)
    {
        isZooming = true;
        forzarSalidaZoom = false;
        esperandoTecla = true;
        objetoDesactivadoActual = objectToDisable;

        originalPosition = transform.position;
        originalRotation = transform.rotation;

        orbitalHabilitadoAntesDelZoom = orbitalCamera != null && orbitalCamera.enabled;
        if (orbitalCamera != null)
            orbitalCamera.enabled = false;
        if (objectToDisable != null)
            objectToDisable.SetActive(false);
        if (faceCameraScript != null)
            faceCameraScript.enabled = false;
        TogglePlayerMovement(false);
        // Hacer zoom hacia el objetivo
        yield return StartCoroutine(SmoothTransition(targetPosition, targetRotation, transitionDuration));
        TogglePlayerMovement(false);


        // Esperar hasta que el jugador pulse la tecla indicada, o hasta que se fuerce la salida desde fuera
        while (!Input.GetKeyDown(exitKey) && !forzarSalidaZoom)
        {
            yield return null;
        }

        // Si llegamos aquí de forma normal (no porque ForzarResetZoom nos haya cortado
        // desde fuera), limpiamos el flag y avisamos a quien esté escuchando (p.ej. para
        // ocultar el indicador de "pulsa E") ANTES de empezar la transición de vuelta,
        // para que la desaparición sea inmediata al pulsar la tecla.
        esperandoTecla = false;
        objetoDesactivadoActual = null;
        OnZoomEndedByKey?.Invoke();

        // Regresar suavemente a la posición original
        yield return StartCoroutine(SmoothTransition(originalPosition, originalRotation, transitionDuration));
        TogglePlayerMovement(true);
        if (orbitalCamera != null)
            orbitalCamera.enabled = orbitalHabilitadoAntesDelZoom;
        if (faceCameraScript != null)
            faceCameraScript.enabled = true;
        if (objectToDisable != null)
            objectToDisable.SetActive(true);
        isZooming = false;
        zoomCoroutine = null;
    }

    IEnumerator SmoothTransition(Vector3 targetPos, Quaternion targetRot, float duration)
    {
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
        transform.rotation = targetRot;
    }

    IEnumerator PermanentTransition(Vector3 targetPos, Quaternion targetRot, float duration)
    {
        if (orbitalCamera != null)
            orbitalCamera.enabled = false;

        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
        transform.rotation = targetRot;

        zoomCoroutine = null;
        isZooming = false;
    }

    // Parar el movimiento del player
    private void TogglePlayerMovement(bool canMove)
    {
        if (player != null)
        {
            var controller = player.GetComponent<PlayerController>();
            if (controller != null)
            {
                controller.canMove = canMove;
            }
        }
    }

    // Activa/desactiva la hitbox del player (para que no pueda colisionar con
    // triggers de interacción) y su gravedad (para que no atraviese el suelo
    // al quedarse sin collider). Se usa durante el zoom del evento, que es
    // "prioritario" y no debe permitir que el jugador interactúe con nada más.
    private void ToggleHitboxYGravedad(bool activo)
    {
        if (playerCollider != null)
            playerCollider.enabled = activo;

        if (playerController != null)
            playerController.SetGravedadActiva(activo);
    }
}