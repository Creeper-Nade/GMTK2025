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

            // 1. �ӱ����б��Ƴ�
            InventoryManager.Instance.RemoveItem(item);

            // 2. ���� slot UI
            //ObjectPoolManager.Instance.ReturnObjectToPool(draggedSlot.gameObject);
            // ��
            Destroy(draggedSlot.gameObject);//���û�ö����
        }
    }

    public void ReceiveItem(InventoryItem item)
    {
        Debug.Log($"����ۣ�{gameObject.name}����Ʒ��{item.itemName}");

        if (previewIcon != null && item.icon != null)
        {
            previewIcon.sprite = item.icon;
            previewIcon.enabled = true;
        }

        // ����Ҳ���Դ��������߼������翪ʼ��ҩ����ϵ�
    }
}