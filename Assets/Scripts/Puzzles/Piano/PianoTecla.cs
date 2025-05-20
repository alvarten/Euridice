using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PianoTecla : MonoBehaviour
{
    public string idTecla;
    public PianoPuzzle puzzleManager;

    private Image imagen;
    private Color colorOriginal;

    void Start()
    {
        imagen = GetComponent<Image>();
        if (imagen != null)
            colorOriginal = imagen.color;
    }

    public void OnClick()
    {
        puzzleManager.PulsarTecla(this);
    }

    public void MostrarFeedback(Color color)
    {
        if (imagen != null)
        {
            StopAllCoroutines(); // por si se pulsa rápido varias veces
            StartCoroutine(CambiarColorTemporal(color));
        }
    }

    private IEnumerator CambiarColorTemporal(Color nuevoColor)
    {
        imagen.color = nuevoColor;
        yield return new WaitForSeconds(0.5f);
        imagen.color = colorOriginal;
    }
}
