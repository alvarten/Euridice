using UnityEngine;
using UnityEngine.EventSystems;

public class ClockHandleDragger : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    public RectTransform centerPoint; // El centro del reloj
    private RectTransform handTransform; // La manecilla a rotar
    public float initialAngleOffset = 0f;

    private float dragOffset = 0f;

    private void Awake()
    {
        handTransform = transform.parent.GetComponent<RectTransform>();
        handTransform.rotation = Quaternion.Euler(0f, 0f, initialAngleOffset);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(centerPoint, Input.mousePosition, eventData.pressEventCamera, out Vector2 pointerPos);
        float pointerAngle = Mathf.Atan2(pointerPos.y, pointerPos.x) * Mathf.Rad2Deg - 90f;

        float currentHandAngle = handTransform.eulerAngles.z;

        dragOffset = Mathf.DeltaAngle(pointerAngle, currentHandAngle); // Diferencia entre mouse y manecilla (el sprite tiene una inclinacion por defecto)
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(centerPoint, Input.mousePosition, eventData.pressEventCamera, out Vector2 pointerPos);
        float pointerAngle = Mathf.Atan2(pointerPos.y, pointerPos.x) * Mathf.Rad2Deg - 90f;

        float finalAngle = pointerAngle + dragOffset;
        handTransform.rotation = Quaternion.Euler(0f, 0f, finalAngle);
    }
}
