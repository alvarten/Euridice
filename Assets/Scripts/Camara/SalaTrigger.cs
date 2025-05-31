using UnityEngine;
using System.Collections;

public class SalaTrigger : MonoBehaviour
{
    [Header("Foco de cámara")]
    public Transform cameraFocusPoint;
    public Vector3 desiredEulerRotation;
    public float delayBeforeZoom = 3f;

    private Coroutine zoomDelayCoroutine;
    private CameraZoomEffect cameraZoom;
    private Transform player;
    private Collider triggerCollider;
    private bool playerInside = false;

    void Start()
    {
        cameraZoom = Camera.main.GetComponent<CameraZoomEffect>();
        triggerCollider = GetComponent<Collider>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    void Update()
    {
        if (player == null || triggerCollider == null) return;

        bool isCurrentlyInside = triggerCollider.bounds.Contains(player.position);

        if (isCurrentlyInside && !playerInside)
        {
            playerInside = true;
            zoomDelayCoroutine = StartCoroutine(WaitForZoomAndStart());
        }
        else if (!isCurrentlyInside && playerInside)
        {
            playerInside = false;

            if (zoomDelayCoroutine != null)
            {
                StopCoroutine(zoomDelayCoroutine);
                zoomDelayCoroutine = null;
            }

            cameraZoom.RestoreOrbitalCamera();
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
