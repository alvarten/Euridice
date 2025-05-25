using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class InventoryItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public string itemId;
    private Image image;
    private CanvasGroup canvasGroup;

    private Transform originalParent;
    private Vector3 originalPosition;

    private bool isZoomed = false;
    private bool isAnimating = false;

    private void Awake()
    {
        image = GetComponent<Image>();
        if (image == null)
            image = GetComponentInChildren<Image>();

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        transform.localScale = Vector3.one;
    }

    public void Setup(Sprite sprite, string id)
    {
        itemId = id;

        if (image != null && sprite != null)
        {
            image.sprite = sprite;
            image.preserveAspect = true;
        }
        else
        {
            Debug.LogWarning($"No se pudo asignar sprite al objeto del inventario: {itemId}");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isZoomed || isAnimating) return;
        originalParent = transform.parent;
        originalPosition = transform.position;
        transform.SetParent(transform.root);
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isZoomed || isAnimating) return;
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isZoomed || isAnimating) return;
        transform.SetParent(originalParent);
        transform.position = originalPosition;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isAnimating) return;

        if (!isZoomed)
            StartCoroutine(ZoomToCenter());
        else
            StartCoroutine(ReturnFromZoom());
    }

    private IEnumerator ZoomToCenter()
    {
        isAnimating = true;
        isZoomed = true;

        originalParent = transform.parent;
        originalPosition = transform.position;

        transform.SetParent(transform.root);

        Vector3 targetPosition = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        Vector3 startPos = transform.position;
        Vector3 startScale = Vector3.one;
        Vector3 targetScale = Vector3.one * 3f;

        float duration = 0.3f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(startPos, targetPosition, t);
            transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        transform.localScale = targetScale;

        isAnimating = false;
    }

    private IEnumerator ReturnFromZoom()
    {
        isAnimating = true;

        Vector3 startPos = transform.position;
        Vector3 startScale = transform.localScale;
        Vector3 targetScale = Vector3.one * 0.62524f;

        float duration = 0.4f; 
        float elapsed = 0f;

        while (elapsed < duration)
        {
            //transform.localScale = targetScale;
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(startPos, originalPosition, t);
            transform.localScale = Vector3.Lerp(startScale, targetScale, t );
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Aseguramos los valores finales exactos
        transform.position = originalPosition;
        transform.localScale = targetScale;

        // Ahora lo devolvemos al layout del inventario
        transform.SetParent(originalParent);

        isZoomed = false;
        isAnimating = false;
    }

}
