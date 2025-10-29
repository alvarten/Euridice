using UnityEngine;
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

    // Mueve la cámara a una posición y rotación específicas con transición suave.
    public void StartZoom(Vector3 targetPosition, Quaternion targetRotation, float transitionDuration, float holdTime)
    {
        if (zoomCoroutine != null)
            StopCoroutine(zoomCoroutine);

        zoomCoroutine = StartCoroutine(ZoomSequence(targetPosition, targetRotation, transitionDuration, holdTime));
    }
    public void StartZoomUntilKey(Vector3 targetPosition, Quaternion targetRotation, float transitionDuration, KeyCode exitKey, GameObject objectToDisable = null)
    {
        if (zoomCoroutine != null)
            StopCoroutine(zoomCoroutine);

        zoomCoroutine = StartCoroutine(ZoomUntilKeySequence(targetPosition, targetRotation, transitionDuration, exitKey, objectToDisable));
    }

    public void CancelZoom()
    {
        if (zoomCoroutine != null)
        {
            StopCoroutine(zoomCoroutine);
            zoomCoroutine = StartCoroutine(SmoothTransition(originalPosition, originalRotation, zoomDuration));

            if (orbitalCamera != null)
                orbitalCamera.enabled = true;

            isZooming = false;
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
        if (zoomCoroutine != null)
            StopCoroutine(zoomCoroutine);

        zoomCoroutine = StartCoroutine(PermanentTransition(targetPosition, targetRotation, duration));
    }

    IEnumerator ZoomSequence(Vector3 targetPosition, Quaternion targetRotation, float transitionDuration, float holdTime)
    {
        isZooming = true;

        originalPosition = transform.position;
        originalRotation = transform.rotation;

        if (orbitalCamera != null)
            orbitalCamera.enabled = false;

        yield return StartCoroutine(SmoothTransition(targetPosition, targetRotation, transitionDuration));

        yield return new WaitForSeconds(holdTime);

        yield return StartCoroutine(SmoothTransition(originalPosition, originalRotation, transitionDuration));

        if (orbitalCamera != null)
            orbitalCamera.enabled = true;

        isZooming = false;
        zoomCoroutine = null;
    }
    IEnumerator ZoomUntilKeySequence(Vector3 targetPosition, Quaternion targetRotation, float transitionDuration, KeyCode exitKey, GameObject objectToDisable)
    {
        isZooming = true;

        originalPosition = transform.position;
        originalRotation = transform.rotation;

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


        // Esperar hasta que el jugador pulse la tecla indicada
        while (!Input.GetKeyDown(exitKey))
        {
            yield return null;
        }

        // Regresar suavemente a la posición original
        yield return StartCoroutine(SmoothTransition(originalPosition, originalRotation, transitionDuration));
        TogglePlayerMovement(true);
        if (orbitalCamera != null)
            orbitalCamera.enabled = true;
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
}