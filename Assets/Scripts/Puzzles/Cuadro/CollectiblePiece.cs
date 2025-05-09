using UnityEngine;

public class CollectiblePiece : MonoBehaviour
{
    public string pieceKey = "PiezaEspada";

    // Esta función se asigna al botón desde el Inspector
    public void OnClickCollect()
    {
        PlayerPrefs.SetInt(pieceKey, 1);
        PlayerPrefs.Save();

        gameObject.SetActive(false); // O Destroy(gameObject);
    }

    void Start()
    {
        // Si la pieza ya fue recogida, volverla a mostrar
        if (PlayerPrefs.GetInt(pieceKey, 0) == 1)
        {
            PlayerPrefs.SetInt(pieceKey, 0);
            gameObject.SetActive(true);
        }
    }
}
