using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDropSlot : MonoBehaviour, IDropHandler
{
    public Image previewIcon;

    private InventoryItem currentItemInSlot = null;
    private InventorySlot currentSlotUI = null;  // ��ǰ��Ʒ��UI Slot

    public void OnDrop(PointerEventData eventData)
    {
        InventorySlot draggedSlot = eventData.pointerDrag?.GetComponent<InventorySlot>();

        if (draggedSlot != null)
        {
            InventoryItem newItem = draggedSlot.GetItem();

            // ���������Ʒ���Ȼص�����
            if (currentItemInSlot != null)
            {
                InventoryManager.Instance.AddItem(currentItemInSlot);

                if (currentSlotUI != null)
                {
                    //ObjectPoolManager.Instance.ReturnObjectToPool(currentSlotUI.gameObject);
                    Destroy(currentSlotUI.gameObject);  // ���û�ö����
                }
            }

            // ��������Ʒ
            ReceiveItem(newItem, draggedSlot);

            // �ӱ����Ƴ�����Ʒ����Ϊ�������ںϳɲ��
            InventoryManager.Instance.RemoveItem(newItem);
            Destroy(draggedSlot.gameObject);
        }
    }

    public void ReceiveItem(InventoryItem item, InventorySlot slotUI)
    {
        Debug.Log($"����ۣ�{gameObject.name}����Ʒ��{item.itemName}");

        if (previewIcon != null && item.icon != null)
        {
            previewIcon.sprite = item.icon;
            previewIcon.preserveAspect = true;  // ����ԭʼ����
            previewIcon.enabled = true;
        }

        // ��¼��ǰ�۵���Ʒ��UI
        currentItemInSlot = item;
        currentSlotUI = slotUI;
    }


    /// ��ѡ����ղ�λ
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
