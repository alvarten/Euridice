using UnityEngine;

public class FloatingIdle : MonoBehaviour
{
    [Header("Movimiento vertical")]
    public float amplitude = 0.25f;   // Altura máxima del movimiento
    public float frequency = 1f;      // Velocidad de la oscilación

    [Header("Mirar a cámara")]
    public bool lookAtCamera = true;  // Activa/desactiva el efecto billboard
    public Camera targetCamera;       // Si se deja vacío, usa Camera.main

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;

        if (targetCamera == null)
            targetCamera = Camera.main;
    }

    void Update()
    {
        // Movimiento vertical tipo seno
        float newY = startPosition.y + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }

    void LateUpdate()
    {
        if (!lookAtCamera || targetCamera == null)
            return;

        // Orienta el cartel hacia la cámara, manteniéndolo vertical (sin inclinarse)
        Vector3 direction = transform.position - targetCamera.transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.0001f)
            transform.rotation = Quaternion.LookRotation(direction);
    }
}