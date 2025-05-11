using UnityEngine;
using UnityEngine.Events;

public class Interactuable : MonoBehaviour
{
    public GameObject visualIndicator;
    public UnityEvent onInteract;

    private bool isPlayerInside = false;
    

    void Start()
    {
        if (visualIndicator != null)
            visualIndicator.SetActive(false);        
    }

    void Update()
    {
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
        if (onInteract != null) { 
            onInteract.Invoke();
        }            
    }
}
