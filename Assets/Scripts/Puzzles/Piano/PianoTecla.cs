using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PianoTecla : MonoBehaviour
{
    public string idTecla;
    public PianoPuzzle puzzleManager;

    private Image imagen;

    void Start()
    {
        imagen = GetComponent<Image>();
    }

    public void OnClick()
    {
        puzzleManager.PulsarTecla(this);
    }
}
