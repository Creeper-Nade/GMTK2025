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

            // ��ѡ������ icon�����Զ�ˢ��
            Destroy(slot.gameObject);
            Debug.Log("������Ʒ: " + item.itemName);
        }
    }
}
