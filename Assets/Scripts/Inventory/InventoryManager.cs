using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    [Header("UI")]
    public GameObject inventoryPanel;
    public Transform contentParent;
    public GameObject slotPrefab;

    [Header("Data")]
    private List<InventoryItem> items = new List<InventoryItem>();

    public void RemoveItem(InventoryItem item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            RefreshUI();
        }
    }

    public void ToggleInventory()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        if (inventoryPanel.activeSelf)
        {
            RefreshUI();
        }
    }

    public void AddItem(InventoryItem newItem)
    {
        InventoryItem existingItem = items.Find(i => i.itemName == newItem.itemName);
        if (existingItem != null)
        {
            existingItem.quantity += newItem.quantity;
        }
        else
        {
            items.Add(newItem);
        }
        RefreshUI();
    }

    private void RefreshUI()
    {
        foreach (Transform child in contentParent)
        {
            ObjectPoolManager.Instance.ReturnObjectToPool(child.gameObject);
        }

        foreach (InventoryItem item in items)
        {
            GameObject slotGO = ObjectPoolManager.Instance.SpawnObject(slotPrefab, contentParent, Quaternion.identity);
            InventorySlot slot = slotGO.GetComponent<InventorySlot>();
            slot.SetItem(item);
        }
    }
}
