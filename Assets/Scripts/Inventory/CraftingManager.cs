using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;  // 引入LINQ排序用

public class CraftingManager : MonoBehaviour
{
    public List<ItemDropSlot> craftingSlots;  // 四个合成槽

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

        // 统计物品名称与数量
        Dictionary<string, int> nameCount = new Dictionary<string, int>();
        foreach (var item in itemsToCombine)
        {
            if (nameCount.ContainsKey(item.itemName))
                nameCount[item.itemName]++;
            else
                nameCount[item.itemName] = 1;
        }

        // 按首字母排序名称
        var sortedNames = nameCount.Keys.OrderBy(name => name).ToList();

        // 拼接合成名称
        string combinedName = "合成物(";
        for (int i = 0; i < sortedNames.Count; i++)
        {
            string name = sortedNames[i];
            int count = nameCount[name];
            combinedName += $"{name}×{count}";
            if (i < sortedNames.Count - 1)
                combinedName += " + ";
        }
        combinedName += ")";

        // 合并冷却时间（最大值）
        float maxCooldown = itemsToCombine.Max(item => item.cooldownTime);

        // 是否闹鬼
        bool isHaunted = itemsToCombine.Any(item => item.isHaunted);

        // 创建新 InventoryItem
        InventoryItem newItem = new InventoryItem(
            name: combinedName,
            iconSprite: itemsToCombine[0].icon,
            col: combinedColor,
            cooldown: maxCooldown,
            haunted: isHaunted,
            qty: 1
        );

        // 提交药水
        PotionSubmitPanel.Instance.SubmitPotion(newItem);

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
