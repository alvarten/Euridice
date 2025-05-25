using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public Transform inventoryPanel;
    public GameObject inventoryItemPrefab;

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
    public bool HasItem(string itemId)
    {
        foreach (Transform child in inventoryPanel)
        {
            InventoryItemUI item = child.GetComponent<InventoryItemUI>();
            if (item != null && item.itemId == itemId)
            {
                return true;
            }
        }
        return false;
    }

    public void RemoveItem(string itemId)
    {
        foreach (Transform child in inventoryPanel)
        {
            InventoryItemUI item = child.GetComponent<InventoryItemUI>();
            if (item != null && item.itemId == itemId)
            {
                Destroy(child.gameObject);
                return;
            }
        }
    }
}