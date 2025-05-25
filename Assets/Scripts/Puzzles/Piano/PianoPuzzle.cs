using System.Collections.Generic;
using UnityEngine;

public class PianoPuzzle : MonoBehaviour
{
    public List<string> secuenciaCorrecta;
    private int progresoActual = 0;

    public GameObject objetoAlCompletar; 
    public SFXPlayer sfxPlayer;
    public void PulsarTecla(PianoTecla tecla)
    {
        sfxPlayer.PlayPianoNote(tecla.idTecla);
        if (tecla.idTecla == secuenciaCorrecta[progresoActual])
        {
            progresoActual++;

            if (progresoActual >= secuenciaCorrecta.Count)
            {
                Debug.Log("¡Puzzle resuelto!");
                PuzzleResuelto();
            }
        }
        else
        {
            progresoActual = 0;
        }
    }

    private void PuzzleResuelto()
    {
        if (objetoAlCompletar != null)
        {
            objetoAlCompletar.SetActive(true);
        }
    }
}
