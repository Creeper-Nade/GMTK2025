using UnityEngine;
using UnityEngine.UI;

public class OrderManager : MonoBehaviour
{
    [SerializeField] private Sprite[] orderSprites;  // 在 Inspector 中拖入多个订单图
    [SerializeField] private Image orderDisplayImage; // Inspector 中拖入 UI Image 组件用于显示订单贴图

    private int currentOrderIndex = -1;

    void Start()
    {
        RefreshOrder();
    }
    public void SetOrderVisible(bool visible)
    {
        Debug.Log("订单 UI 显示状态：" + visible);
        if (orderDisplayImage != null)
            orderDisplayImage.gameObject.SetActive(visible);
    }
    public void RefreshOrder()
    {
        if (orderSprites.Length == 0 || orderDisplayImage == null)
        {
            Debug.LogWarning("订单贴图数组为空或未设置展示 Image");
            return;
        }

        int newIndex;
        do
        {
            newIndex = Random.Range(0, orderSprites.Length);
        } while (newIndex == currentOrderIndex && orderSprites.Length > 1); // 保证不会和上次一样

        currentOrderIndex = newIndex;
        orderDisplayImage.sprite = orderSprites[currentOrderIndex];
    }

    public void SubmitPotion()
    {
        // 你可以在这里添加提交逻辑，比如判断药剂是否匹配订单等

        Debug.Log("药剂提交成功，刷新订单！");
        RefreshOrder();
    }
}
