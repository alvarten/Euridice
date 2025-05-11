using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public Transform inventoryPanel; // El layout horizontal en la esquina inferior izquierda
    public GameObject inventoryItemPrefab; // Prefab con Image + drag script

    private void Awake()
    {
        Instance = this;
    }

    public void AddItem(Sprite itemSprite, string itemId)
    {
        GameObject newItem = Instantiate(inventoryItemPrefab, inventoryPanel);
        InventoryItemUI itemUI = newItem.GetComponent<InventoryItemUI>();
        itemUI.Setup(itemSprite, itemId);
    }
}
