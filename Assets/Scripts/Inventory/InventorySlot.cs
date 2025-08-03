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

    private bool isTooltipVisible = false;  // �����ֶ�

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            canvas = FindFirstObjectByType<Canvas>();

        }

        if (canvas == null)
        {
            Debug.LogError("δ�ҵ��κ� Canvas����ק������");
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

        // ����Ƿ���������Ч��λ
        GameObject dropTarget = eventData.pointerEnter;

        if (dropTarget != null && dropTarget.GetComponent<ItemDropSlot>() != null)
        {
            // ���óɹ���֪ͨĿ��۴�����Ʒ
            //dropTarget.GetComponent<ItemDropSlot>().ReceiveItem(currentItem);
            dropTarget.GetComponent<ItemDropSlot>().ReceiveItem(currentItem, this);


            // ��ѡ�����ػ��Ƴ��� slot��������գ�
            InventoryManager.Instance.RemoveItem(currentItem);
        }
        else
        {
            // δ���ã���λ
            transform.SetParent(originalParent);
            transform.localPosition = Vector3.zero;
        }

        isDragging = false;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (isDragging) return;

        if (!ItemTooltip.IsVisible)
        {
            string info = $"{currentItem.itemName}\nCooldown Time: {currentItem.cooldownTime:F1}s";
            Vector2 mousePos = eventData.position;
            ItemTooltip.Instance.ShowTooltip(info, mousePos);
        }
        else
        {
            ItemTooltip.Instance.HideTooltip();
        }
    }


    public InventoryItem GetItem()
    {
        return currentItem;
    }

    /*public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentItem != null)
        {
            string info = $"{currentItem.itemName}\nCooldown Time: {currentItem.cooldownTime:F1}s";
            Vector2 mousePos = eventData.position;
            ItemTooltip.Instance.ShowTooltip(info, mousePos);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ItemTooltip.Instance.HideTooltip();
    }*/
}
