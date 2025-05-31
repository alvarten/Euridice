using UnityEngine;
using System.Collections;

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

    [Header("Opcional: Restaurar Cámara")]
    public bool restoreAfterDelay = false;
    public float restoreDelay = 3.5f;

    public void ActivarZoom()
    {
        Quaternion focusRotation = Quaternion.Euler(eulerRotation);

        if (zoomEffect != null)
            zoomEffect.StartZoom(focusPoint, focusRotation, zoomDuration, zoomHoldTime);

        StartCoroutine(DisableFaceCameraTemporarily((zoomDuration*2) + zoomHoldTime));

        if (restoreAfterDelay)
            StartCoroutine(RestoreOrbitalAfterDelay(restoreDelay));
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
}
