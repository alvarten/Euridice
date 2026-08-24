using UnityEngine;

// Va en cada uno de los 5 GameObjects de los cuadros.
// Requiere un Collider en este mismo GameObject (puede ser trigger) para que OnMouseDown funcione.
// Empieza siempre deshabilitado: solo CuadrosPuzzleController lo activa mientras dura el zoom.
public class CuadroClickable : MonoBehaviour
{
    [Tooltip("El mismo id que antes pasabas manualmente a VelasManager.ActivarVela(idVela) desde el Interactuable de este cuadro.")]
    public int idVela;

    public VelasManager velasManager;

    private void Awake()
    {
        // Nos aseguramos de que arranca deshabilitado pase lo que pase con el checkbox del Inspector.
        enabled = false;
    }

    private void OnMouseDown()
    {
        if (velasManager != null)
            velasManager.ActivarVela(idVela);
    }
}