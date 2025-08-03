using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CraftingManager : MonoBehaviour
{
    public List<ItemDropSlot> craftingSlots;  // 四个合成槽
    //public Image resultPreviewIcon;

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

        // 合并颜色
        Color combinedColor = CombineColors(itemsToCombine);

        // 合并名称
        string combinedName = "合成物(";
        for (int i = 0; i < itemsToCombine.Count; i++)
        {
            combinedName += itemsToCombine[i].itemName;
            if (i < itemsToCombine.Count - 1) combinedName += "+";
        }
        combinedName += ")";

        // 合并冷却时间（最大值）
        float maxCooldown = 0f;
        foreach (var item in itemsToCombine)
        {
            if (item.cooldownTime > maxCooldown)
                maxCooldown = item.cooldownTime;
        }

        // 是否闹鬼
        bool isHaunted = false;
        foreach (var item in itemsToCombine)
        {
            if (item.isHaunted)
            {
                isHaunted = true;
                break;
            }
        }

        //创建 InventoryItem
        InventoryItem newItem = new InventoryItem(
            name: combinedName,
            iconSprite: itemsToCombine[0].icon,
            col: combinedColor,
            cooldown: maxCooldown,
            haunted: isHaunted,
            qty: 1
        );

        //提交药水给 PotionSubmitPanel
        PotionSubmitPanel.Instance.SubmitPotion(newItem);

        // 显示预览图标（可选）
        /*if (resultPreviewIcon != null)
        {
            resultPreviewIcon.sprite = newItem.icon;
            resultPreviewIcon.color = combinedColor;
            resultPreviewIcon.enabled = true;
        }*/

        // 清空槽位
        foreach (var slot in craftingSlots)
        {
            slot.ClearCraftSlot();
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
