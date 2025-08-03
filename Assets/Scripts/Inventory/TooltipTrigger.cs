using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private InventorySlot inventorySlot;

    private void Awake()
    {
        inventorySlot = GetComponentInParent<InventorySlot>();
        if (inventorySlot == null)
        {
            Debug.LogWarning("TooltipTrigger 找不到父物体的 InventorySlot");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (inventorySlot == null || inventorySlot.GetItem() == null) return;

        var item = inventorySlot.GetItem();
        string info = $"{item.itemName}\nCooldown Time: {item.cooldownTime:F1}s";
        Vector2 mousePos = eventData.position;
        ItemTooltip.Instance.ShowTooltip(info, mousePos);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ItemTooltip.Instance.HideTooltip();
    }
}
