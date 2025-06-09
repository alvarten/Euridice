using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [Header("Tiempo en segundos antes de cambiar de escena")]
    public float tiempoEspera = 5f;

    [Header("Índice de la escena a la que se cambiará")]
    public int indiceEscenaDestino = 1;

    private void Start()
    {
        StartCoroutine(CambiarEscenaTrasEspera());
    }

    private IEnumerator CambiarEscenaTrasEspera()
    {
        yield return new WaitForSeconds(tiempoEspera);
        SceneManager.LoadScene(indiceEscenaDestino);
    }
}
