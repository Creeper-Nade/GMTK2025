using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDropSlot : MonoBehaviour, IDropHandler
{
    public Image previewIcon;

    private InventoryItem currentItemInSlot = null;
    private InventorySlot currentSlotUI = null;  // 当前物品的UI Slot

    private void Awake()
    {
        SetIconAlpha(0f);
    }

    public void OnDrop(PointerEventData eventData)
    {
        InventorySlot draggedSlot = eventData.pointerDrag?.GetComponent<InventorySlot>();

        if (draggedSlot != null)
        {
            InventoryItem newItem = draggedSlot.GetItem();

            // 如果已有物品，先回到背包
            if (currentItemInSlot != null)
            {
                InventoryManager.Instance.AddItem(currentItemInSlot);

                if (currentSlotUI != null)
                {
                    Destroy(currentSlotUI.gameObject);  // 如果没用对象池
                }
            }

            // 接收新物品
            ReceiveItem(newItem, draggedSlot);

            // 从背包移除新物品（因为它现在在合成槽里）
            InventoryManager.Instance.RemoveItem(newItem);
            Destroy(draggedSlot.gameObject);
        }
    }

    public void ReceiveItem(InventoryItem item, InventorySlot slotUI)
    {
        Debug.Log($"放入槽：{gameObject.name}，物品：{item.itemName}");

        if (previewIcon != null && item.icon != null)
        {
            previewIcon.sprite = item.icon;
            previewIcon.preserveAspect = true;
            previewIcon.enabled = true;

            // 设置alpha为255（完全可见）
            SetIconAlpha(1f);
        }

        currentItemInSlot = item;
        currentSlotUI = slotUI;
    }

    public void ClearSlot()
    {
        currentItemInSlot = null;
        currentSlotUI = null;
        previewIcon.sprite = null;

        // 设置alpha为0（完全透明）
        SetIconAlpha(0f);
    }

    public void ClearCraftSlot()
    {
        currentItemInSlot = null;
        currentSlotUI = null;
        previewIcon.sprite = null;

        // 设置alpha为0（完全透明）
        SetIconAlpha(0f);
    }

    public InventoryItem GetCurrentItem()
    {
        return currentItemInSlot;
    }

    // 🔹 工具函数：设置 Image 的 alpha 值
    private void SetIconAlpha(float alpha)
    {
        if (previewIcon != null)
        {
            Color color = previewIcon.color;
            color.a = alpha;
            previewIcon.color = color;
        }
    }
}
