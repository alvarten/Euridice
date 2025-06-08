using UnityEngine;
using System.Collections;

public class InvocarContextoTutorial : MonoBehaviour
{
    [Header("Referencia al PuzzleManager")]
    public PuzzleManager puzzleManager;

    [Header("Panel a mostrar")]
    public GameObject panel;

    [Header("Tag del jugador")]
    public string playerTag = "Player";

    [Header("Fade")]
    public float fadeDuration = 1.5f;

    private bool yaActivado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (yaActivado) return;

        if (other.CompareTag(playerTag))
        {
            if (puzzleManager != null && panel != null)
            {
                puzzleManager.TogglePuzzlePanel(panel);

                CanvasGroup canvasGroup = panel.GetComponent<CanvasGroup>();
                if (canvasGroup != null)
                {
                    canvasGroup.alpha = 0f;
                    canvasGroup.interactable = false;
                    canvasGroup.blocksRaycasts = false;

                    panel.SetActive(true);
                    StartCoroutine(FadeInCanvasGroup(canvasGroup));
                }
                else
                {
                    Debug.LogWarning("El panel no tiene un componente CanvasGroup.");
                }

                yaActivado = true;
            }
            else
            {
                Debug.LogWarning("Falta asignar PuzzleManager o panel en InvocarContextoTutorial.");
            }
        }
    }

    IEnumerator FadeInCanvasGroup(CanvasGroup canvasGroup)
    {
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
}
