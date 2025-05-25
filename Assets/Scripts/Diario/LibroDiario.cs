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
    }

    private void ActualizarBotones()
    {
        botonAnterior.interactable = paginaActual > 0;
        botonSiguiente.interactable = paginaActual + 2 < paginaSprites.Length;
    }
}
