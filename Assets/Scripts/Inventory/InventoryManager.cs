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
        // 依据 itemName + color + cooldown + haunted 判断是否相同
        InventoryItem existingItem = items.Find(i =>
            i.itemName == newItem.itemName &&
            i.color == newItem.color &&
            Mathf.Approximately(i.cooldownTime, newItem.cooldownTime) &&
            i.isHaunted == newItem.isHaunted
        );

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
        // 回收所有旧槽位,目前还是动态
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
