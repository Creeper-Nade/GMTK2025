// CraftingManager.cs
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CraftingManager : MonoBehaviour
{
    public List<ItemDropSlot> craftingSlots;  // 四个合成槽
    public Image resultPreviewIcon;

    public GameObject potionPrefab;  // 预制体，用于生成 PotionItem
    public Transform potionParent;   // 放置生成的 PotionItem

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

        // 生成 PotionItem
        GameObject newPotionGO = Instantiate(potionPrefab, potionParent);
        PotionItem potionComponent = newPotionGO.GetComponent<PotionItem>();
        if (potionComponent != null)
        {
            potionComponent.SetPotion(
                sprite: itemsToCombine[0].icon,  // 可定制图标
                id: combinedName,
                color: combinedColor,
                cooldown: maxCooldown,
                haunted: isHaunted
            );
        }

        // 显示预览图标
        if (resultPreviewIcon != null)
        {
            resultPreviewIcon.sprite = itemsToCombine[0].icon;
            resultPreviewIcon.color = combinedColor;
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
