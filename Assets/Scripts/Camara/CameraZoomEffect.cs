using UnityEngine;
using System.Collections;

public class CameraZoomEffect : MonoBehaviour
{
    [Header("Zoom temporal")]
    public float zoomDuration = 1.5f;
    public float holdDuration = 2f;

    [Header("Componentes externos")]
    public OrbitalCamera orbitalCamera;
    public MonoBehaviour faceCameraScript;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private Coroutine zoomCoroutine;
    private bool isZooming = false;

    public void StartZoom(Vector3 targetPosition, Quaternion targetRotation)
    {
        if (zoomCoroutine != null)
            StopCoroutine(zoomCoroutine);

        zoomCoroutine = StartCoroutine(ZoomSequence(targetPosition, targetRotation));
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

    
    // Mueve la cámara a una posición y rotación específicas con transición suave e indefinida.    
    public void SetCameraToPositionSmooth(Vector3 targetPosition, Quaternion targetRotation, float duration)
    {
        if (zoomCoroutine != null)
            StopCoroutine(zoomCoroutine);

        zoomCoroutine = StartCoroutine(PermanentTransition(targetPosition, targetRotation, duration));
    }

    IEnumerator ZoomSequence(Vector3 targetPosition, Quaternion targetRotation)
    {
        isZooming = true;

        originalPosition = transform.position;
        originalRotation = transform.rotation;

        if (orbitalCamera != null)
            orbitalCamera.enabled = false;

        yield return StartCoroutine(SmoothTransition(targetPosition, targetRotation, zoomDuration));

        yield return new WaitForSeconds(holdDuration);

        yield return StartCoroutine(SmoothTransition(originalPosition, originalRotation, zoomDuration));

        if (orbitalCamera != null)
            orbitalCamera.enabled = true;

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
}
