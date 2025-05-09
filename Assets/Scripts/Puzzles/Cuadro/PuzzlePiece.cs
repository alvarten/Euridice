using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PuzzlePiece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Identificador de posición lógica")]
    public string correctId; // Por ejemplo, "A1", "B1", "C1", "A2"...

    [HideInInspector] public Transform originalParent;

    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Vector3 originalPosition;
    private PuzzleManager puzzleManager;

    private GameObject placeholder;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
        puzzleManager = FindObjectOfType<PuzzleManager>();
        if (puzzleManager == null)
        {
            Debug.LogError("No se encontró el PuzzleManager en la escena");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        originalPosition = rectTransform.position;

        // Crear placeholder
        placeholder = new GameObject("Placeholder");
        RectTransform rt = placeholder.AddComponent<RectTransform>();
        rt.SetParent(originalParent);
        rt.SetSiblingIndex(transform.GetSiblingIndex());

        // Copiar tamaño si hay LayoutElement
        LayoutElement le = placeholder.AddComponent<LayoutElement>();
        LayoutElement sourceLE = GetComponent<LayoutElement>();
        if (sourceLE != null)
        {
            le.preferredWidth = sourceLE.preferredWidth;
            le.preferredHeight = sourceLE.preferredHeight;
            le.flexibleWidth = 0;
            le.flexibleHeight = 0;
        }

        transform.SetParent(canvas.transform); // Sacar del GridLayoutGroup
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        transform.SetParent(originalParent); // Volver al grid para colocación final

        GameObject target = eventData.pointerCurrentRaycast.gameObject;

        if (target != null && target.CompareTag("PuzzlePiece") && target != gameObject)
        {
            PuzzlePiece targetPiece = target.GetComponent<PuzzlePiece>();

            // Obtener los índices de las piezas a intercambiar
            int myIndex = placeholder.transform.GetSiblingIndex();
            int targetIndex = targetPiece.transform.GetSiblingIndex();

            // Evitar la reestructuración incorrecta, especialmente al mover hacia abajo.
            if (myIndex == targetIndex)
            {
                Debug.Log("No se realiza intercambio, las piezas son iguales.");
                return; // Si las piezas son iguales, no hacemos nada
            }

            // Intercambiar las posiciones de las piezas visualmente
            transform.SetSiblingIndex(targetIndex);
            targetPiece.transform.SetSiblingIndex(myIndex);

            // Intercambiar los correctId de las piezas
            string tempId = correctId;
            correctId = targetPiece.correctId;
            targetPiece.correctId = tempId;

            // Comprobar si el puzzle está resuelto tras el intercambio
            if (puzzleManager != null)
            {
                puzzleManager.CheckIfSolved(); // Llamamos a la comprobación después de hacer el intercambio
            }
        }
        else
        {
            Debug.Log($"Soltado fuera de una pieza válida, regresando al índice original.");
            transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
        }

        Destroy(placeholder);
        rectTransform.localPosition = Vector3.zero;
    }

    // Método para verificar si la pieza está en su lugar correcto
    public bool IsInCorrectPosition()
    {
        // Compara el correctId actualizado con el nombre de la pieza
        return correctId == gameObject.name.Replace("PuzzlePieceCuadro", "");
    }
}
