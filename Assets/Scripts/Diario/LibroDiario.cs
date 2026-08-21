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

    [Header("Sección de Pistas")]
    public Sprite[] paginaSpritesPistas;

    [Header("Cierre del Diario")]
    [Tooltip("Se usa para cerrar el diario reutilizando el sistema de paneles ya existente (CloseCurrentPuzzle).")]
    public PuzzleManager puzzleManager;

    private bool enModoPistas = false;
    public bool EstaEnModoPistas => enModoPistas;

    void Start()
    {
        MostrarPaginas();
        ActualizarBotones();
    }

    void Update()
    {
        if (enModoPistas)
        {
            // Viendo pistas: E cierra solo esa subsección
            if (Input.GetKeyDown(KeyCode.E))
            {
                CerrarPistas();
            }
        }
        else
        {
            // Diario normal: E cierra el diario entero, reutilizando el sistema de paneles del PuzzleManager
            if (Input.GetKeyDown(KeyCode.E) && puzzleManager != null)
            {
                puzzleManager.CloseCurrentPuzzle();
            }
        }
    }

    public void PaginaSiguiente()
    {
        if (enModoPistas) return; // navegación bloqueada mientras se ven las pistas

        if (paginaActual + 2 < paginaSprites.Length)
        {
            paginaActual += 2;
            MostrarPaginas();
            ActualizarBotones();
        }
    }

    public void PaginaAnterior()
    {
        if (enModoPistas) return; // navegación bloqueada mientras se ven las pistas

        if (paginaActual - 2 >= 0)
        {
            paginaActual -= 2;
            MostrarPaginas();
            ActualizarBotones();
        }
    }

    // Llamado desde MarcapaginasPistas al hacer click sobre el marcapáginas
    public void TogglePistas()
    {
        if (enModoPistas)
            CerrarPistas();
        else
            AbrirPistas();
    }

    public void AbrirPistas()
    {
        if (enModoPistas) return;

        if (paginaSpritesPistas == null || paginaSpritesPistas.Length < 2)
        {
            Debug.LogWarning("LibroDiario: paginaSpritesPistas necesita al menos 2 sprites (izquierda y derecha).");
            return;
        }

        enModoPistas = true;

        // No tocamos paginaActual para nada: pintamos directamente los sprites de pistas.
        paginaIzquierda.sprite = paginaSpritesPistas[0];
        paginaDerecha.sprite = paginaSpritesPistas[1];

        if (objetoActivarEnPagina3 != null)
            objetoActivarEnPagina3.SetActive(false);

        // Bloqueamos los botones de navegación normal mientras se ven las pistas
        botonAnterior.interactable = false;
        botonSiguiente.interactable = false;
    }

    public void CerrarPistas()
    {
        if (!enModoPistas) return;

        enModoPistas = false;

        // paginaActual nunca cambió, así que esto simplemente repinta la página en la que ya estabas
        MostrarPaginas();
        ActualizarBotones();
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

        // Activar objeto si estamos en página 3 (índice 2)
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
            // Si estamos en esa página (y no estamos viendo las pistas encima), actualizar la vista
            if (paginaActual == 2 && !enModoPistas)
                MostrarPaginas();
        }
    }
}