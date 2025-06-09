using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [Header("Tiempo en segundos antes de cambiar de escena")]
    public float tiempoEspera = 5f;

    [Header("�ndice de la escena a la que se cambiar�")]
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
