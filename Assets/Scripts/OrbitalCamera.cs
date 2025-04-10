using UnityEngine;

public class OrbitalCamera : MonoBehaviour
{
    public Transform player;
    public Transform bookCenter;
    public float distance = 10f;
    public float height = 5f;

    void LateUpdate()
    {
        if (player == null || bookCenter == null) return;

        // Direcci�n horizontal del centro hacia el jugador (sin inclinaci�n)
        Vector3 flatDirection = player.position - bookCenter.position;
        flatDirection.y = 0f;
        flatDirection.Normalize();

        // Posici�n de c�mara: orbita alrededor, pero siempre a misma altura
        Vector3 newPos = player.position - flatDirection * distance + Vector3.up * height;

        transform.position = newPos;
        transform.LookAt(player.position);
    }
}
