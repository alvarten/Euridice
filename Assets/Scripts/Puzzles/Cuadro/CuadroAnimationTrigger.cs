using UnityEngine;

public class CuadroAnimationTrigger : MonoBehaviour
{
    [Header("Dependencias")]
    public Animator cuadroAnimator;
    public GameObject resolvedPanel;

    [Header("Inventario y recolectable")]
    public string requiredItemId = "PiezaEspada";
    public string rewardItemId = "LlaveCajaFuerte";
    public Sprite rewardItemIcon;
    public SFXPlayer sfxPlayer;

    [Header("Objetos a activar/desactivar")]
    public GameObject interactuableCuadro;
    public GameObject interactuableCajaFuerte;

    private bool hasTriggered = false;
    private bool rewardCollected = false;

    void Update()
    {
        if (hasTriggered) return;

        if (InventoryManager.Instance != null &&
            InventoryManager.Instance.HasItem(requiredItemId) &&
            resolvedPanel != null &&
            !resolvedPanel.activeInHierarchy)
        {
            if (cuadroAnimator != null)
            {
                cuadroAnimator.SetTrigger("CuadroCae");
                sfxPlayer.PlayLock();
                hasTriggered = true;
                Debug.Log("Animación del cuadro activada al recoger la pieza de espada.");
            }

            if (interactuableCuadro != null)
                interactuableCuadro.SetActive(false);

            if (interactuableCajaFuerte != null)
                interactuableCajaFuerte.SetActive(true);
        }
    }

    // Llamar a este método al interactuar con la caja fuerte
    public void CollectRewardFromCajaFuerte()
    {
        if (rewardCollected) return;

        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddItem(rewardItemIcon, rewardItemId);
            if (sfxPlayer != null)
                sfxPlayer.PlayPick();

            rewardCollected = true;
            Debug.Log($"Objeto '{rewardItemId}' añadido al inventario.");
        }

        if (interactuableCajaFuerte != null)
            interactuableCajaFuerte.SetActive(false);
    }
}
