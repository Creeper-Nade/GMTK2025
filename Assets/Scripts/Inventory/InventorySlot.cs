using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
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
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(originalParent);
        transform.localPosition = Vector3.zero;
        canvasGroup.blocksRaycasts = true;
        isDragging = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isDragging) return; // 忽略拖拽状态的点击

        Debug.Log("点击物品: " + currentItem.itemName);
        // 执行查看、使用物品等逻辑
    }

    public InventoryItem GetItem()
    {
        return currentItem;
    }
}
