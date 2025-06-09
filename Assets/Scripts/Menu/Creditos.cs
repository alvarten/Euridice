using UnityEngine;

public class Creditos : MonoBehaviour
{
    [Header("Velocidad de movimiento hacia arriba")]
    public float velocidad = 20f;

    [Header("Retraso antes de comenzar")]
    public float retardoInicio = 3f;

    private float tiempoTranscurrido = 0f;
    private bool puedeMoverse = false;

    void Update()
    {
        if (!puedeMoverse)
        {
            tiempoTranscurrido += Time.deltaTime;
            if (tiempoTranscurrido >= retardoInicio)
            {
                puedeMoverse = true;
            }
            return;
        }

        transform.Translate(Vector3.up * velocidad * Time.deltaTime);
    }
}
