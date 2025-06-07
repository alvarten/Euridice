using UnityEngine;
using System.Collections;
using TMPro;

public class MesillaDiario : MonoBehaviour
{
    [Header("Texto a mostrar")]
    public TextMeshProUGUI uiText;
    public string message = "Texto de ejemplo";

    [Header("Duraciones")]
    public float fadeInDuration = 1f;
    public float displayDuration = 2f;
    public float fadeOutDuration = 1f;

    [Header("Objeto a activar")]
    public GameObject objectToActivate;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        if (uiText == null)
        {
            Debug.LogError("No se asignó ningún Text UI.");
            return;
        }

        canvasGroup = uiText.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = uiText.gameObject.AddComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;
    }

    public void ShowMessage()
    {
        uiText.text = message;
        StartCoroutine(ShowAndActivate());
    }

    private IEnumerator ShowAndActivate()
    {
        // Fade In
        yield return StartCoroutine(FadeTo(1f, fadeInDuration));

        // Activar objeto
        if (objectToActivate != null)
            objectToActivate.SetActive(true);

        // Mantener texto visible
        yield return new WaitForSeconds(displayDuration);

        // Fade Out
        yield return StartCoroutine(FadeTo(0f, fadeOutDuration));
        
    }

    private IEnumerator FadeTo(float targetAlpha, float duration)
    {
        float startAlpha = canvasGroup.alpha;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
    }
}
