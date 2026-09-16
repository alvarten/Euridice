using UnityEngine;
using UnityEngine.Events;

public class Interactuable : MonoBehaviour
{
    public GameObject visualIndicator;
    public UnityEvent onInteract;
    public float interactionRadius = 2f;

    private bool isPlayerInside = false;
    private Transform player;

    void Start()
    {
        if (visualIndicator != null)
            visualIndicator.SetActive(false);

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    void Update()
    {
        if (player != null && isPlayerInside)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance > interactionRadius)
            {
                isPlayerInside = false;
                if (visualIndicator != null)
                    visualIndicator.SetActive(false);
            }
        }

        // Solo bloqueamos la E durante el zoom "prioritario" del evento del 70%.
        // A propósito NO usamos canMove aquí: canMove también está en false durante
        // los puzles (donde la E se usa para salir) y durante otras interacciones,
        // y bloquear por canMove rompería esos flujos.
        if (isPlayerInside && !CameraZoomEffect.EventoActivo && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            if (visualIndicator != null)
                visualIndicator.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            if (visualIndicator != null)
                visualIndicator.SetActive(false);
        }
    }

    private void Interact()
    {
        if (onInteract != null)
            onInteract.Invoke();
    }
}