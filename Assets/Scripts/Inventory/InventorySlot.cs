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

        // 左键点击：使用物品（可扩展）
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
        Debug.Log($"使用物品：{currentItem.itemName}");
        // TODO: 物品使用逻辑
    }

    private void DiscardItem()
    {
        Debug.Log($"丢弃物品：{currentItem.itemName}");

        // 直接移除这个物品
        InventoryManager.Instance.RemoveItem(currentItem);
    }
}
