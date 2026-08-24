using System.Collections.Generic;
using UnityEngine;

public class VelasManager : MonoBehaviour
{
    [Header("Orden correcto de velas (IDs)")]
    public List<int> ordenCorrecto = new List<int> { 0, 1, 2, 3, 4 };

    [Header("Objetos que se activan al encender cada vela")]
    public GameObject[] objetosDeVela; // Deberían haber 5 objetos, uno por vela
    private int progresoActual = 0;
    private bool puzzleResuelto = false;

    [Header("Objetos interactuables que se desactivan al completar el puzle")]
    public GameObject[] objetosInteractuables;

    [Header("Objeto que se activa al completar el puzle")]
    public GameObject objetosInteractuablesFinal;

    [Header("Objeto con script OpenDoor")]
    public OpenDoor cuadroFinal;
    public SFXPlayer sfxPlayer;

    [Header("Salida automática del zoom al completar")]
    [Tooltip("El mismo CameraZoomEffect que usa CuadrosPuzzleController para el zoom de este minijuego.")]
    public CameraZoomEffect zoomEffect;

    [Tooltip("El GameObject de la zona de aproximación (el que tiene el Interactuable que dispara el zoom). Se destruye al completar el puzle para que no se pueda volver a entrar.")]
    public GameObject zonaInteractuableCuadros;

    [Header("Destrucción de interactuables")]
    public ObjectDestroyer objectDestroyer;

    // Esta función se llama cuando se interactúa con una vela
    public void ActivarVela(int idVela)
    {
        if (puzzleResuelto) return;

        // Si es la vela correcta
        if (idVela == ordenCorrecto[progresoActual])
        {
            // Sonido de encender
            sfxPlayer?.PlayClick();

            // Encender objeto asociado
            if (idVela >= 0 && idVela < objetosDeVela.Length)
            {
                objetosDeVela[idVela].SetActive(true);
            }

            progresoActual++;

            // Comprobar si se ha completado el puzle
            if (progresoActual >= ordenCorrecto.Count)
            {
                puzzleResuelto = true;
                Debug.Log("¡Puzle resuelto!");
                OnPuzzleCompletado();
            }
        }
        else
        {
            // Error: reiniciar
            Debug.Log("Orden incorrecto. Reiniciando puzle...");
            ReiniciarPuzle();
            //SONIDO ERROR
            sfxPlayer.PlayError();
        }
    }

    private void ReiniciarPuzle()
    {
        progresoActual = 0;
        // Apagar todos los objetos
        foreach (GameObject obj in objetosDeVela)
        {
            obj.SetActive(false);
        }
    }

    private void OnPuzzleCompletado()
    {
        // Desactivar objetos interactuables
        foreach (GameObject obj in objetosInteractuables)
        {
            if (obj != null)
                obj.SetActive(false);
        }

        // Activar objeto final
        if (objetosInteractuablesFinal != null)
        {
            objetosInteractuablesFinal.SetActive(true);
        }

        // Activar abrir cuadro
        if (cuadroFinal != null)
        {
            // Sonido de encender
            sfxPlayer?.PlayDoor();
            cuadroFinal.AbrirPuerta();
        }

        // Destruir la zona que da acceso al minijuego, para que no se pueda volver a entrar
        if (zonaInteractuableCuadros != null)
        {
            if (objectDestroyer != null)
                objectDestroyer.DestroyObject(zonaInteractuableCuadros);
            else
                Debug.LogWarning("VelasManager: no hay ObjectDestroyer asignado, no se pudo destruir zonaInteractuableCuadros.");
        }

        // Forzar la salida del zoom automáticamente (reutiliza toda la secuencia normal
        // de salida: restaurar cámara, movimiento del jugador, y el evento OnZoomEndedByKey)
        if (zoomEffect != null)
        {
            zoomEffect.ForzarSalidaZoom();
        }
    }
}