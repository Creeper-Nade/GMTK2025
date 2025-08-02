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
    public int maxSlots = 6;  // 背包最多 6 个格子

    private void Start()
    {
        inventoryPanel.SetActive(false);
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
        if (items.Count >= maxSlots)
        {
            Debug.LogWarning("背包已满，无法添加更多物品！");
            return;
        }

        newItem.quantity = 1;  // 每个 slot 仅存 1 个
        items.Add(newItem);
        RefreshUI();
    }

    public void RemoveItem(InventoryItem item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            RefreshUI();
        }
    }

    private void RefreshUI()
    {
        // 回收所有旧槽位
        foreach (Transform child in contentParent)
        {
            ObjectPoolManager.Instance.ReturnObjectToPool(child.gameObject);
        }

        // 创建新槽位
        foreach (InventoryItem item in items)
        {
            GameObject slotGO = ObjectPoolManager.Instance.SpawnObject(slotPrefab, contentParent, Quaternion.identity);
            InventorySlot slot = slotGO.GetComponent<InventorySlot>();
            slot.SetItem(item);
        }
    }
}
