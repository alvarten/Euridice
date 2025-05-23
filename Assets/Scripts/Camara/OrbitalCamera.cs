using UnityEngine;

public class OrbitalCamera : MonoBehaviour
{
    public Transform player;
    public Transform bookCenter;

    [Header("Ajustes de posición")]
    public float baseDistance = 10f;           // Distancia base desde el centro del libro
    public float minPlayerDistance = 3f;       // Distancia mínima permitida entre cámara y jugador
    public float height = 5f;                  // Altura de la cámara

    [Header("Suavizado")]
    public float positionSmoothSpeed = 5f;
    public float rotationSmoothSpeed = 10f;

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (player == null || bookCenter == null) return;

        // Dirección horizontal del centro del libro hacia el jugador
        Vector3 flatDirection = player.position - bookCenter.position;
        flatDirection.y = 0f;
        flatDirection.Normalize();

        // Posición base de la cámara desde el centro del libro
        Vector3 baseCameraPos = bookCenter.position + flatDirection * baseDistance + Vector3.up * height;

        // Verifica si el jugador está demasiado cerca de la cámara
        Vector3 toPlayer = player.position - baseCameraPos;
        float distanceToPlayer = toPlayer.magnitude;

        if (distanceToPlayer < minPlayerDistance)
        {
            // Alejar la cámara manteniendo dirección
            Vector3 escapeDir = -toPlayer.normalized;
            baseCameraPos = player.position + escapeDir * minPlayerDistance;
            baseCameraPos.y = bookCenter.position.y + height;
        }

        // Aplicar suavizado al movimiento de cámara
        transform.position = Vector3.SmoothDamp(transform.position, baseCameraPos, ref velocity, 1f / positionSmoothSpeed);

        // Rotar suavemente hacia un punto ligeramente por encima del jugador
        Vector3 lookTarget = Vector3.Lerp(player.position, player.position + Vector3.up * 2f, 0.3f);
        Quaternion desiredRot = Quaternion.LookRotation(lookTarget - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRot, Time.deltaTime * rotationSmoothSpeed);
    }
}
