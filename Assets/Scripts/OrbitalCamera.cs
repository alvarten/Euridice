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

        // Dirección horizontal del centro hacia el jugador (sin inclinación)
        Vector3 flatDirection = player.position - bookCenter.position;
        flatDirection.y = 0f;
        flatDirection.Normalize();

        // Posición de cámara: orbita alrededor, pero siempre a misma altura
        Vector3 newPos = player.position - flatDirection * distance + Vector3.up * height;

        transform.position = newPos;
        transform.LookAt(player.position);
    }
}
