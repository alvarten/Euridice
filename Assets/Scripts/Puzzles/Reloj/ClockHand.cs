using UnityEngine;
using UnityEngine.EventSystems;

public class ClockHand : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    public bool isHourHand = false;
    private RectTransform rectTransform;
    private Vector2 center;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform.parent as RectTransform, Input.mousePosition, eventData.pressEventCamera, out center);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pointerPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform.parent as RectTransform, Input.mousePosition, eventData.pressEventCamera, out pointerPos);

        Vector2 direction = pointerPos - center;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        rectTransform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public float GetRotationAngle()
    {
        // Retorna el ángulo entre 0 y 360
        return (360f - rectTransform.eulerAngles.z) % 360f;
    }
}
