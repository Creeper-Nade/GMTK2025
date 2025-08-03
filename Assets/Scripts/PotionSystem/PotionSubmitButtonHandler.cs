using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PotionSubmitButtonHandler : MonoBehaviour
{
    public List<ItemDropSlot> craftingSlots; // 拖入你的四个槽位

    public void Submit()
    {
        List<InventoryItem> items = new List<InventoryItem>();
        foreach (var slot in craftingSlots)
        {
            var item = slot.GetCurrentItem();
            if (item != null) items.Add(item);
        }

        if (items.Count > 0)
        {
            PotionSubmitPanel.Instance.SubmitPotions(items);
        }
        else
        {
            Debug.Log("没有有效物品提交。");
        }
    }
}
