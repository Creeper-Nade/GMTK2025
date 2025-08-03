using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDropSlot : MonoBehaviour, IDropHandler
{
    public Image previewIcon;

    private InventoryItem currentItemInSlot = null;
    private InventorySlot currentSlotUI = null;  // 当前物品的UI Slot

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
                    //ObjectPoolManager.Instance.ReturnObjectToPool(currentSlotUI.gameObject);
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
            previewIcon.preserveAspect = true;  // 保持原始比例
            previewIcon.enabled = true;
        }

        // 记录当前槽的物品和UI
        currentItemInSlot = item;
        currentSlotUI = slotUI;
    }


    /// 可选：清空槽位
    public void ClearSlot()
    {
        currentItemInSlot = null;
        currentSlotUI = null;
        previewIcon.sprite = null;
        previewIcon.enabled = false;
    }

    public void ClearCraftSlot()
    {
        currentItemInSlot = null;
        currentSlotUI = null;
        previewIcon.sprite = null;
        //previewIcon.enabled = false;
    }

    public InventoryItem GetCurrentItem()
    {
        return currentItemInSlot;
    }

}
