using UnityEngine;

public class PlayerTeleporter : MonoBehaviour
{
    [Header("Referencia al jugador")]
    [SerializeField] private Transform player;
    [SerializeField] private OrbitalCamera orbitalCamera;

    public void TeleportTo(Transform destino)
    {
        if (player == null || destino == null)
        {
            Debug.LogWarning("Teleport fallido: Faltan referencias.");
            return;
        }

        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null)
            cc.enabled = false; // Desactiva momentáneamente

        player.position = destino.position;
        orbitalCamera.SnapToTarget();
        if (cc != null)
            cc.enabled = true; // Reactiva

        Debug.Log($"Jugador transportado a: {destino.name}");
    }
}
