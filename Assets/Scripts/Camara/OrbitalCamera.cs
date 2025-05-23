using UnityEngine;

public class OrbitalCamera : MonoBehaviour
{
    public Transform player;
    public Transform bookCenter;

    [Header("Ajustes de posici�n")]
    public float baseDistance = 10f;           // Distancia base desde el centro del libro
    public float minPlayerDistance = 3f;       // Distancia m�nima permitida entre c�mara y jugador
    public float height = 5f;                  // Altura de la c�mara

    [Header("Suavizado")]
    public float positionSmoothSpeed = 5f;
    public float rotationSmoothSpeed = 10f;

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (player == null || bookCenter == null) return;

        // Direcci�n horizontal del centro del libro hacia el jugador
        Vector3 flatDirection = player.position - bookCenter.position;
        flatDirection.y = 0f;
        flatDirection.Normalize();

        // Posici�n base de la c�mara desde el centro del libro
        Vector3 baseCameraPos = bookCenter.position + flatDirection * baseDistance + Vector3.up * height;

        // Verifica si el jugador est� demasiado cerca de la c�mara
        Vector3 toPlayer = player.position - baseCameraPos;
        float distanceToPlayer = toPlayer.magnitude;

        if (distanceToPlayer < minPlayerDistance)
        {
            // Alejar la c�mara manteniendo direcci�n
            Vector3 escapeDir = -toPlayer.normalized;
            baseCameraPos = player.position + escapeDir * minPlayerDistance;
            baseCameraPos.y = bookCenter.position.y + height;
        }

        // Aplicar suavizado al movimiento de c�mara
        transform.position = Vector3.SmoothDamp(transform.position, baseCameraPos, ref velocity, 1f / positionSmoothSpeed);

        // Rotar suavemente hacia un punto ligeramente por encima del jugador
        Vector3 lookTarget = Vector3.Lerp(player.position, player.position + Vector3.up * 2f, 0.3f);
        Quaternion desiredRot = Quaternion.LookRotation(lookTarget - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRot, Time.deltaTime * rotationSmoothSpeed);
    }
}
