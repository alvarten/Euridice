using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

// Este script va en el propio GameObject del marcapáginas (el que tiene el icono del ojo).
// Requisitos en ese GameObject:
//  - Un componente Image (o cualquier Graphic) con "Raycast Target" activado, para poder recibir el hover/click.
//  - Debe estar dentro de un Canvas con GraphicRaycaster, y la escena debe tener un EventSystem (viene por defecto en cualquier Canvas de Unity).
[RequireComponent(typeof(RectTransform))]
public class MarcapaginasPistas : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Referencias")]
    public LibroDiario libroDiario;

    [Header("Animación de hover")]
    public Vector2 desplazamientoHover = new Vector2(15f, 0f);
    public float duracionAnimacion = 0.15f;

    private RectTransform rectTransform;
    private Vector2 posicionReposo;
    private Coroutine coroutineActual;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        posicionReposo = rectTransform.anchoredPosition;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        IniciarAnimacionHacia(posicionReposo + desplazamientoHover);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IniciarAnimacionHacia(posicionReposo);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (libroDiario != null)
            libroDiario.TogglePistas();
    }

    private void IniciarAnimacionHacia(Vector2 destino)
    {
        if (coroutineActual != null)
            StopCoroutine(coroutineActual);
        coroutineActual = StartCoroutine(AnimarPosicion(destino));
    }

    private IEnumerator AnimarPosicion(Vector2 destino)
    {
        Vector2 inicio = rectTransform.anchoredPosition;
        float t = 0f;
        while (t < duracionAnimacion)
        {
            t += Time.deltaTime;
            rectTransform.anchoredPosition = Vector2.Lerp(inicio, destino, t / duracionAnimacion);
            yield return null;
        }
        rectTransform.anchoredPosition = destino;
    }
}