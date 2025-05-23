using UnityEngine;
using System.Collections;

public class SalaTrigger : MonoBehaviour
{
    public Transform cameraFocusPoint;
    public Vector3 desiredEulerRotation;
    public float delayBeforeZoom = 3f;

    private Coroutine zoomDelayCoroutine;
    private CameraZoomEffect cameraZoom;

    void Start()
    {
        cameraZoom = Camera.main.GetComponent<CameraZoomEffect>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            zoomDelayCoroutine = StartCoroutine(DelayZoom());
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (zoomDelayCoroutine != null)
                StopCoroutine(zoomDelayCoroutine);

            cameraZoom.RestoreOrbitalCamera(); // <-- activamos control orbital
        }
    }

    IEnumerator DelayZoom()
    {
        yield return new WaitForSeconds(delayBeforeZoom);
        if (cameraFocusPoint != null)
        {
            cameraZoom.SetCameraToPositionSmooth(
                cameraFocusPoint.position,
                Quaternion.Euler(desiredEulerRotation),
                1f
            );
        }
    }
}
