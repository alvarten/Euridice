using System.Collections.Generic;
using UnityEngine;

public class PianoPuzzle : MonoBehaviour
{
    public List<string> secuenciaCorrecta;
    private int progresoActual = 0;

    public GameObject objetoAlCompletar; // El objeto que se mostrará
    public SFXPlayer sfxPlayer;
    public void PulsarTecla(PianoTecla tecla)
    {
        sfxPlayer.PlayPianoNote(tecla.idTecla);
        if (tecla.idTecla == secuenciaCorrecta[progresoActual])
        {
            tecla.MostrarFeedback(Color.green);
            progresoActual++;

            if (progresoActual >= secuenciaCorrecta.Count)
            {
                Debug.Log("¡Puzzle resuelto!");
                PuzzleResuelto();
            }
        }
        else
        {
            tecla.MostrarFeedback(Color.red);
            progresoActual = 0;
        }
    }

    private void PuzzleResuelto()
    {
        if (objetoAlCompletar != null)
        {
            objetoAlCompletar.SetActive(true);
        }

        // Aquí podrías también desactivar las teclas o bloquear interacción si quieres
    }
}
