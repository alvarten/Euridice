using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    [Header("Componentes")]
    public SFXPlayer sfxPlayer;

    [Header("Rotación")]
    public float rotationAmount = 40f;
    public float rotationSpeed = 120f; // grados por segundo

    [Header("Una sola vez")]
    public bool openOnlyOnce = true;

    private bool hasOpened = false;
    private bool isRotating = false;
    private Quaternion targetRotation;

    void Start()
    {
        // Asegura que el objeto esté activo al iniciar la escena
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
            }
        }
    }

    public void AbrirPuerta()
    {
        if (openOnlyOnce && hasOpened)
            return;

        // Guardamos la rotación objetivo sumando los grados deseados
        targetRotation = Quaternion.Euler(0, transform.eulerAngles.y + rotationAmount, 0);
        isRotating = true;

        if (sfxPlayer != null)
            sfxPlayer.PlayDoor();

        hasOpened = true;
    }
}
