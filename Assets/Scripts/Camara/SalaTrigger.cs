using UnityEngine;
using System.Collections;

public class SalaTrigger : MonoBehaviour
{
    public Transform cameraFocusPoint;
    public Vector3 desiredEulerRotation;
    public float delayBeforeZoom = 3f;

    private Coroutine zoomDelayCoroutine;
    private CameraZoomEffect cameraZoom;
    private bool playerInside = false;

    void Start()
    {
        cameraZoom = Camera.main.GetComponent<CameraZoomEffect>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            zoomDelayCoroutine = StartCoroutine(WaitForZoomAndStart());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            if (zoomDelayCoroutine != null)
            {
                StopCoroutine(zoomDelayCoroutine);
                zoomDelayCoroutine = null;
            }

            cameraZoom.RestoreOrbitalCamera(); // Reactivamos el control orbital
        }
    }

    IEnumerator WaitForZoomAndStart()
    {
        // Esperamos el delay inicial
        yield return new WaitForSeconds(delayBeforeZoom);

        // Esperamos hasta que no haya zoom activo o hasta que el jugador salga del trigger
        while (cameraZoom.isZooming && playerInside)
        {
            yield return null;
        }

        // Solo iniciamos el zoom si el jugador sigue dentro y hay foco
        if (playerInside && cameraFocusPoint != null)
        {
            cameraZoom.SetCameraToPositionSmooth(
                cameraFocusPoint.position,
                Quaternion.Euler(desiredEulerRotation),
                1f
            );
        }
    }
}
