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

    // InventoryManager.cs

    public Sprite redHerbIcon;
    public Sprite blueHerbIcon;
    public Sprite greenHerbIcon;
    public Sprite yellowHerbIcon;
    public Sprite purpleHerbIcon;

    public void AddMaterialByColor(HerbColor color)
    {
        InventoryItem item = null;

        switch (color)
        {
            case HerbColor.Red:
                item = new InventoryItem("Red Herb", redHerbIcon, 1);
                break;
            case HerbColor.Blue:
                item = new InventoryItem("Blue Herb", blueHerbIcon, 1);
                break;
            case HerbColor.Green:
                item = new InventoryItem("Green Herb", greenHerbIcon, 1);
                break;
            case HerbColor.Yellow:
                item = new InventoryItem("Yellow Herb", yellowHerbIcon, 1);
                break;
            case HerbColor.Purple:
                item = new InventoryItem("Purple Herb", purpleHerbIcon, 1);
                break;
            default:
                Debug.LogWarning("未知颜色");
                return;
        }

        AddItem(item);
    }


    public void Start()
    {
        inventoryPanel.SetActive(false);  
    }


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
