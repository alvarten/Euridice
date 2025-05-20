using UnityEngine;
using System.Collections;

public class CameraZoomEffect : MonoBehaviour
{
    public float zoomDuration = 1.5f;    // Tiempo de transición al zoom
    public float holdDuration = 2f;      // Tiempo que permanece enfocado

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private OrbitalCamera orbitalCamera;

    private bool isZooming = false;
    public MonoBehaviour faceCameraScript; 


    void Start()
    {
        orbitalCamera = GetComponent<OrbitalCamera>();
    }

    // Llamar a este método desde otro script
    public void StartZoom(Vector3 targetPosition, Quaternion targetRotation)
    {
        if (!isZooming)
        {
            StartCoroutine(ZoomSequence(targetPosition, targetRotation));
        }
    }

    IEnumerator ZoomSequence(Vector3 targetPosition, Quaternion targetRotation)
    {
        isZooming = true;

        // Guardar estado original
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        // Desactivar orbital
        if (orbitalCamera != null)
            orbitalCamera.enabled = false;
        // Desactivar faceCamera
        if (faceCameraScript != null)
            faceCameraScript.enabled = false;

        // Ir al punto
        yield return StartCoroutine(SmoothTransition(targetPosition, targetRotation, zoomDuration));

        // Esperar enfocado
        yield return new WaitForSeconds(holdDuration);

        // Volver
        yield return StartCoroutine(SmoothTransition(originalPosition, originalRotation, zoomDuration));

        // Reactivar orbital
        if (orbitalCamera != null)
            orbitalCamera.enabled = true;
        // Reactivar faceCamera
        if (faceCameraScript != null)
            faceCameraScript.enabled = true;

        isZooming = false;
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
}
