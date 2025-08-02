using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CraftingManager : MonoBehaviour
{
    public List<ItemDropSlot> craftingSlots;  // 四个槽位
    public Image resultPreviewIcon;

    public void CombineItems()
    {
        List<InventoryItem> itemsToCombine = new List<InventoryItem>();

        foreach (var slot in craftingSlots)
        {
            InventoryItem item = slot.GetCurrentItem();
            if (item != null)
            {
                itemsToCombine.Add(item);
            }
        }

        if (itemsToCombine.Count == 0)
        {
            Debug.Log("没有物品用于合成。");
            return;
        }

        // 计算平均颜色
        Color combinedColor = CombineColors(itemsToCombine);

        // 合成新物品名称
        string combinedName = "合成物(";
        for (int i = 0; i < itemsToCombine.Count; i++)
        {
            combinedName += itemsToCombine[i].itemName;
            if (i < itemsToCombine.Count - 1) combinedName += "+";
        }
        combinedName += ")";

        // 生成新物品
        InventoryItem newItem = new InventoryItem(
            name: combinedName,
            iconSprite: itemsToCombine[0].icon,  // 取第一个图标或换自定义
            col: combinedColor,
            cooldown: 0f,
            haunted: false,
            qty: 1
        );

        // 添加到背包
        InventoryManager.Instance.AddItem(newItem);

        // 显示预览
        if (resultPreviewIcon != null)
        {
            resultPreviewIcon.sprite = newItem.icon;
            resultPreviewIcon.color = newItem.color;
            resultPreviewIcon.enabled = true;
        }

        // 清空槽位
        foreach (var slot in craftingSlots)
        {
            slot.ClearSlot();
        }

        Debug.Log("合成成功：" + combinedName);
    }

    private Color CombineColors(List<InventoryItem> items)
    {
        float r = 0, g = 0, b = 0, a = 0;
        foreach (var item in items)
        {
            r += item.color.r;
            g += item.color.g;
            b += item.color.b;
            a += item.color.a;
        }

        int count = items.Count;
        return new Color(r / count, g / count, b / count, a / count);
    }
}
