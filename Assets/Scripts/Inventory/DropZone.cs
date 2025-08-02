using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        InventorySlot slot = eventData.pointerDrag?.GetComponent<InventorySlot>();
        if (slot != null)
        {
            InventoryItem item = slot.GetItem();
            InventoryManager.Instance.RemoveItem(item);

            // 可选：销毁 icon，或自动刷新
            Destroy(slot.gameObject);
            Debug.Log("丢弃物品: " + item.itemName);
        }
    }
}
