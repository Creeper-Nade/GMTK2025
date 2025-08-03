using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public InventoryItem currentItem;
    public Image iconImage;

    private Transform originalParent;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    private bool isDragging = false;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            canvas = FindFirstObjectByType<Canvas>();
        }

        if (canvas == null)
        {
            Debug.LogError("未找到任何 Canvas，拖拽将出错！");
        }

        canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void SetItem(InventoryItem item)
    {
        currentItem = item;
        iconImage.sprite = item.icon;
        iconImage.enabled = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        originalParent = transform.parent;
        transform.SetParent(canvas.transform);
        canvasGroup.blocksRaycasts = false;

        // 拖拽开始时隐藏 Tooltip
        ItemTooltip.Instance.HideTooltip();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        GameObject dropTarget = eventData.pointerEnter;

        if (dropTarget != null && dropTarget.GetComponent<ItemDropSlot>() != null)
        {
            dropTarget.GetComponent<ItemDropSlot>().ReceiveItem(currentItem, this);
            InventoryManager.Instance.RemoveItem(currentItem);
        }
        else
        {
            transform.SetParent(originalParent);
            transform.localPosition = Vector3.zero;
        }

        isDragging = false;
    }

    public InventoryItem GetItem()
    {
        return currentItem;
    }
}
