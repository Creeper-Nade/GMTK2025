using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDropSlot : MonoBehaviour, IDropHandler
{
    public Image previewIcon;

    public void OnDrop(PointerEventData eventData)
    {
        InventorySlot draggedSlot = eventData.pointerDrag?.GetComponent<InventorySlot>();

        if (draggedSlot != null)
        {
            InventoryItem item = draggedSlot.GetItem();

            ReceiveItem(item);

            // 1. 从背包列表移除
            InventoryManager.Instance.RemoveItem(item);

            // 2. 回收 slot UI
            //ObjectPoolManager.Instance.ReturnObjectToPool(draggedSlot.gameObject);
            // 或
            Destroy(draggedSlot.gameObject);//如果没用对象池
        }
    }

    public void ReceiveItem(InventoryItem item)
    {
        Debug.Log($"放入槽：{gameObject.name}，物品：{item.itemName}");

        if (previewIcon != null && item.icon != null)
        {
            previewIcon.sprite = item.icon;
            previewIcon.enabled = true;
        }

        // 这里也可以触发具体逻辑，比如开始炼药、组合等
    }
}