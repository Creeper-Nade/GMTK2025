using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public Image icon;
    public Text quantityText;

    private InventoryItem currentItem;

    public void SetItem(InventoryItem item)
    {
        currentItem = item;

        icon.sprite = item.icon;
        icon.enabled = true;

        quantityText.text = item.quantity > 1 ? item.quantity.ToString() : "";

        // ��������ʹ����Ʒ������չ��
        Button btn = GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(UseItem);
    }

    public void Clear()
    {
        icon.sprite = null;
        icon.enabled = false;
        quantityText.text = "";
        currentItem = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            DiscardItem();
        }
    }

    private void UseItem()
    {
        Debug.Log($"ʹ����Ʒ��{currentItem.itemName}");
        // TODO: ��Ʒʹ���߼�
    }

    private void DiscardItem()
    {
        Debug.Log($"������Ʒ��{currentItem.itemName}");

        // ֱ���Ƴ������Ʒ
        InventoryManager.Instance.RemoveItem(currentItem);
    }
}
