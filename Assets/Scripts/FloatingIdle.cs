using UnityEngine;

public class FloatingIdle : MonoBehaviour
{
    [Header("Movimiento vertical")]
    public float amplitude = 0.25f;   // Altura máxima del movimiento
    public float frequency = 1f;      // Velocidad de la oscilación

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Movimiento vertical tipo seno
        float newY = startPosition.y + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
