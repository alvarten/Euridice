using UnityEngine;

public class OrbitalCamera : MonoBehaviour
{
    public Transform player;
    public Transform bookCenter;

    [Header("Ajustes de posici�n")]
    public float baseDistance = 10f;
    public float minPlayerDistance = 3f;
    public float heightOffset = 1.2f;

    [Header("Suavizado")]
    public float positionSmoothSpeed = 5f;
    public float rotationSmoothSpeed = 10f;

    private Vector3 velocity = Vector3.zero;
    private bool forceSnap = false;

    void LateUpdate()
    {
        if (player == null || bookCenter == null) return;

        // Direcci�n horizontal del centro del libro hacia el jugador
        Vector3 flatDirection = player.position - bookCenter.position;
        flatDirection.y = 0f;
        flatDirection.Normalize();

        // Posici�n base de la c�mara desde el centro del libro
        float targetHeight = player.position.y + heightOffset;
        Vector3 baseCameraPos = bookCenter.position + flatDirection * baseDistance;
        baseCameraPos.y = targetHeight;

        // Verifica si el jugador est� demasiado cerca de la c�mara
        Vector3 toPlayer = player.position - baseCameraPos;
        float distanceToPlayer = toPlayer.magnitude;

        if (distanceToPlayer < minPlayerDistance)
        {
            Vector3 escapeDir = -toPlayer.normalized;
            baseCameraPos = player.position + escapeDir * minPlayerDistance;
            baseCameraPos.y = targetHeight;
        }

        // Suavizado de movimiento
        if (forceSnap)
        {
            transform.position = baseCameraPos;
            forceSnap = false;
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, baseCameraPos, ref velocity, 1f / positionSmoothSpeed);
        }

        // Suavizado de rotaci�n hacia un punto sobre el jugador
        Vector3 lookTarget = Vector3.Lerp(player.position, player.position + Vector3.up * 2f, 0.3f);
        Quaternion desiredRot = Quaternion.LookRotation(lookTarget - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRot, Time.deltaTime * rotationSmoothSpeed);
    }
    public void SnapToTarget()
    {
        forceSnap = true;
    }
}
