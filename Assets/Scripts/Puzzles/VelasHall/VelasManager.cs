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

    public SFXPlayer sfxPlayer;
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
    }
}
