using UnityEngine;
using UnityEngine.UI;

public class LibroDiario : MonoBehaviour
{
    public Image paginaIzquierda;
    public Image paginaDerecha;

    public Sprite[] paginaSprites;

    private int paginaActual = 0;

    public Button botonSiguiente;
    public Button botonAnterior;

    [Header("Opcional")]
    public GameObject objetoActivarEnPagina3;

    void Start()
    {
        MostrarPaginas();
        ActualizarBotones();
    }

    public void PaginaSiguiente()
    {
        if (paginaActual + 2 < paginaSprites.Length)
        {
            paginaActual += 2;
            MostrarPaginas();
            ActualizarBotones();
        }
    }

    public void PaginaAnterior()
    {
        if (paginaActual - 2 >= 0)
        {
            paginaActual -= 2;
            MostrarPaginas();
            ActualizarBotones();
        }
    }

    private void MostrarPaginas()
    {
        if (paginaActual < paginaSprites.Length)
            paginaIzquierda.sprite = paginaSprites[paginaActual];
        else
            paginaIzquierda.sprite = null;

        if (paginaActual + 1 < paginaSprites.Length)
            paginaDerecha.sprite = paginaSprites[paginaActual + 1];
        else
            paginaDerecha.sprite = null;

        // Activar objeto si estamos en página 3 (indice 2)
        if (objetoActivarEnPagina3 != null)
        {
            objetoActivarEnPagina3.SetActive(paginaActual == 2);
        }
    }

    private void ActualizarBotones()
    {
        botonAnterior.interactable = paginaActual > 0;
        botonSiguiente.interactable = paginaActual + 2 < paginaSprites.Length;
    }

    // Método para cambiar el sprite de la página 3 (índice 2)
    public void CambiarSpritePagina3(Sprite nuevoSprite)
    {
        if (paginaSprites != null && paginaSprites.Length > 2)
        {
            paginaSprites[3] = nuevoSprite;
            // Si estamos en esa página, actualizar la vista
            if (paginaActual == 2)
                MostrarPaginas();
        }
    }
}
