using UnityEngine;

public class OpenTrampilla : MonoBehaviour
{
    [Header("Componentes")]
    public SFXPlayer sfxPlayer;

    [Header("Rotación")]
    public float rotationAmount = 90f; // grados en total (por defecto hacia abajo)
    public float rotationSpeed = 120f; // grados por segundo
    public Vector3 rotationAxis = Vector3.right; // Eje X para trampilla

    [Header("Acciones al terminar")]
    public GameObject objetoOcultar;
    public GameObject objetoMostrar;

    [Header("Una sola vez")]
    public bool openOnlyOnce = true;

    private bool hasOpened = false;
    private bool isRotating = false;
    private Quaternion targetRotation;

    void Start()
    {
        gameObject.SetActive(true);
    }

    void Update()
    {
        if (isRotating)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                transform.rotation = targetRotation;
                isRotating = false;

                // Ejecutar las acciones una vez que terminó de rotar
                if (objetoOcultar != null)
                    objetoOcultar.SetActive(false);

                if (objetoMostrar != null)
                    objetoMostrar.SetActive(true);
            }
        }
    }

    public void AbrirTrampilla()
    {
        if (openOnlyOnce && hasOpened)
            return;

        Vector3 currentEuler = transform.eulerAngles;
        Vector3 axis = rotationAxis.normalized;
        Vector3 newEuler = currentEuler + new Vector3(
            axis.x * rotationAmount,
            axis.y * rotationAmount,
            axis.z * rotationAmount
        );

        targetRotation = Quaternion.Euler(newEuler);
        isRotating = true;

        if (sfxPlayer != null)
            sfxPlayer.PlayDoor();

        hasOpened = true;
    }
}
