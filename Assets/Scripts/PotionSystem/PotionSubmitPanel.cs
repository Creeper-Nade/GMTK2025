using System.Collections.Generic;
using UnityEngine;

public class PotionSubmitPanel : MonoBehaviour
{
    public Transform slotContainer;
    public GameObject potionPrefab;

    private List<GameObject> submittedPotions = new List<GameObject>();
    public static PotionSubmitPanel Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void SubmitPotion(InventoryItem item)
    {
        GameObject newPotion = Instantiate(potionPrefab, slotContainer);
        PotionItem potionItem = newPotion.GetComponent<PotionItem>();
        potionItem.SetPotion(item.icon, item.itemName, item.color, item.cooldownTime, item.isHaunted);
        submittedPotions.Add(newPotion);
    }

    // ✅ 这是新加的方法：支持从多个材料合成出一个 PotionItem，带最大冷却和闹鬼属性
    public void SubmitPotions(List<InventoryItem> items)
    {
        if (items == null || items.Count == 0)
        {
            Debug.LogWarning("提交失败：没有材料");
            return;
        }

        // 名字拼接
        string potionID = "合成物(";
        for (int i = 0; i < items.Count; i++)
        {
            potionID += items[i].itemName;
            if (i < items.Count - 1) potionID += "+";
        }
        potionID += ")";

        // 取第一个图标（也可自定义图标）
        Sprite icon = items[0].icon;

        // 平均颜色
        Color avgColor = CombineColors(items);

        // 最大冷却时间
        float maxCD = 0f;
        foreach (var item in items)
            maxCD = Mathf.Max(maxCD, item.cooldownTime);

        // 是否闹鬼
        bool haunted = false;
        foreach (var item in items)
        {
            if (item.isHaunted)
            {
                haunted = true;
                break;
            }
        }

        // 创建 PotionItem
        GameObject newPotion = Instantiate(potionPrefab, slotContainer);
        PotionItem potionItem = newPotion.GetComponent<PotionItem>();
        potionItem.SetPotion(icon, potionID, avgColor, maxCD, haunted);
        submittedPotions.Add(newPotion);
    }

    public void TriggerHauntOnAll()
    {
        foreach (GameObject potion in submittedPotions)
        {
            if (potion.TryGetComponent<IHauntAction>(out var haunter))
            {
                haunter.Haunt();
            }
        }
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
