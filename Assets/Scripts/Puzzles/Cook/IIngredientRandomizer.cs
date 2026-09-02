using UnityEngine;

public class IngredientRandomizer : MonoBehaviour
{
    [System.Serializable]
    public class IngredienteCandidato
    {
        [Tooltip("Solo para identificarlo en el Inspector, no afecta a la lógica.")]
        public string nombreDebug;

        [Tooltip("Debe coincidir EXACTAMENTE con el itemId configurado en el InventoryAdd del ingrediente físico correspondiente en la escena.")]
        public string ingredienteItemId;

        [Tooltip("Debe coincidir EXACTAMENTE con el itemId configurado en el InventoryAdd de la pista/trozo de hoja correspondiente en la escena.")]
        public string pistaItemId;

        [Tooltip("Sprite de la página del diario que se muestra cuando este es el ingrediente correcto.")]
        public Sprite paginaCompletaSprite;

        [Tooltip("Imagen específica de este ingrediente que se muestra en la olla al depositarlo (usada tanto en la ranura dedicada como en la olla directa).")]
        public GameObject panelDespues;

        [Tooltip("El GameObject del coleccionable/trigger en el mundo (el trozo de hoja). Solo el del ingrediente elegido quedará activo.")]
        public GameObject coleccionableEnEscena;
    }

    [Header("Ingredientes candidatos (se elige 1 al azar)")]
    public IngredienteCandidato[] ingredientesCandidatos;

    [Header("Ranura dedicada en la olla")]
    [Tooltip("La ItemReceiverCook dedicada a ESTE ingrediente variable (las otras dos ranuras de la receta no se tocan).")]
    public ItemReceiverCook ranuraIngredienteVariable;

    [Header("Olla (recepción directa)")]
    public OllaReceiver ollaReceiver;
    [Tooltip("Índice dentro de OllaReceiver.ingredientesAceptados que corresponde a este ingrediente variable.")]
    public int indiceEnOlla;

    [Header("Diario")]
    [Tooltip("La hoja del diario que revela cuál es el ingrediente correcto.")]
    public DiarioItemReceiver diarioItemReceiver;

    [Header("Debug")]
    [Tooltip("Deja en -1 para randomizar. Usa 0, 1 o 2 para forzar un ingrediente concreto en pruebas.")]
    public int forzarIndice = -1;

    void Awake()
    {
        if (ingredientesCandidatos == null || ingredientesCandidatos.Length == 0)
        {
            Debug.LogWarning("IngredientRandomizer: no hay ingredientes candidatos asignados.");
            return;
        }

        int indiceElegido = (forzarIndice >= 0 && forzarIndice < ingredientesCandidatos.Length)
            ? forzarIndice
            : Random.Range(0, ingredientesCandidatos.Length);

        for (int i = 0; i < ingredientesCandidatos.Length; i++)
        {
            if (ingredientesCandidatos[i].coleccionableEnEscena != null)
                ingredientesCandidatos[i].coleccionableEnEscena.SetActive(i == indiceElegido);
        }

        IngredienteCandidato elegido = ingredientesCandidatos[indiceElegido];

        // Ranura dedicada en la olla
        if (ranuraIngredienteVariable != null)
        {
            ranuraIngredienteVariable.acceptedItemId = elegido.ingredienteItemId;
            ranuraIngredienteVariable.panelDespues = elegido.panelDespues;
        }

        // Recepción directa en la olla (misma lógica, entrada identificada por índice)
        if (ollaReceiver != null && ollaReceiver.ingredientesAceptados != null &&
            indiceEnOlla >= 0 && indiceEnOlla < ollaReceiver.ingredientesAceptados.Count)
        {
            OllaReceiver.IngredienteAceptado entry = ollaReceiver.ingredientesAceptados[indiceEnOlla];
            entry.itemId = elegido.ingredienteItemId;
            entry.panelDespues = elegido.panelDespues;
        }
        else if (ollaReceiver != null)
        {
            Debug.LogWarning("IngredientRandomizer: indiceEnOlla fuera de rango en OllaReceiver.ingredientesAceptados.");
        }

        // Diario: tanto el ID de la pista que acepta como el sprite que revela
        if (diarioItemReceiver != null)
        {
            diarioItemReceiver.acceptedItemId = elegido.pistaItemId;
            diarioItemReceiver.nuevoSpritePagina3 = elegido.paginaCompletaSprite;
        }

        Debug.Log($"IngredientRandomizer: ingrediente correcto esta partida = {elegido.nombreDebug}");
    }
}