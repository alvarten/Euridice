using System.Collections.Generic;
using UnityEngine;

public class PianoPuzzle : MonoBehaviour
{
    public List<string> secuenciaCorrecta;
    private int progresoActual = 0;

    public GameObject objetoAlCompletar; // El objeto que se mostrar�
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
                Debug.Log("�Puzzle resuelto!");
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

        // Aqu� podr�as tambi�n desactivar las teclas o bloquear interacci�n si quieres
    }
}
