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

        if (isPlayerInside && Input.GetKeyDown(KeyCode.E))
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
