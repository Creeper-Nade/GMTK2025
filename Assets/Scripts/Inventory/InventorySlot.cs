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
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        // 检测是否拖入了有效槽位
        GameObject dropTarget = eventData.pointerEnter;

        if (dropTarget != null && dropTarget.GetComponent<ItemDropSlot>() != null)
        {
            // 放置成功，通知目标槽处理物品
            //dropTarget.GetComponent<ItemDropSlot>().ReceiveItem(currentItem);
            dropTarget.GetComponent<ItemDropSlot>().ReceiveItem(currentItem, this);


            // 可选：隐藏或移除此 slot（例如回收）
            InventoryManager.Instance.RemoveItem(currentItem);
        }
        else
        {
            // 未放置，归位
            transform.SetParent(originalParent);
            transform.localPosition = Vector3.zero;
        }

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
