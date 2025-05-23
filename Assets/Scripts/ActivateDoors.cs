using UnityEngine;

public class ActivateDoors : MonoBehaviour
{
    [Header("Activar puertas al inicio")]
    public GameObject[] puertasADespertar;

    void Start()
    {
        foreach (GameObject puerta in puertasADespertar)
        {
            if (puerta != null && !puerta.activeSelf)
            {
                puerta.SetActive(true);
            }
        }
    }
}
