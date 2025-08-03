using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionSubmitPanel : MonoBehaviour
{
    public Transform slotContainer;         // UI容器
    public GameObject potionPrefab;         // 药剂预制体
    private List<GameObject> submittedPotions = new List<GameObject>();

    public static PotionSubmitPanel Instance;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        foreach (Transform child in slotContainer)
        {
            if (child.TryGetComponent<PotionItem>(out var potionItem))
            {
                submittedPotions.Add(child.gameObject);
                Debug.Log($"已注册药剂: {potionItem.potionID}");
            }
        }
    }

    public void SubmitPotion(InventoryItem item)
    {
        GameObject newPotion = Instantiate(potionPrefab, slotContainer);
        PotionItem potionItem = newPotion.GetComponent<PotionItem>();
        potionItem.SetPotion(item.icon, item.itemName, item.color, item.cooldownTime, item.isHaunted);
        submittedPotions.Add(newPotion);
    }

    public void SubmitPotions(List<InventoryItem> items)
    {
        if (items == null || items.Count == 0)
        {
            Debug.LogWarning("提交失败：没有材料");
            return;
        }

        string potionID = "合成物(";
        for (int i = 0; i < items.Count; i++)
        {
            potionID += items[i].itemName;
            if (i < items.Count - 1) potionID += "+";
        }
        potionID += ")";

        Sprite icon = items[0].icon;
        Color avgColor = CombineColors(items);

        float maxCD = 0f;
        bool haunted = false;
        foreach (var item in items)
        {
            if (item.isHaunted) haunted = true;
            maxCD = Mathf.Max(maxCD, item.cooldownTime);
        }

        GameObject newPotion = Instantiate(potionPrefab, slotContainer);
        PotionItem potionItem = newPotion.GetComponent<PotionItem>();
        potionItem.SetPotion(icon, potionID, avgColor, maxCD, haunted);
        submittedPotions.Add(newPotion);
    }

    public void TriggerHauntOnAll()
    {
        Debug.Log($"触发闹鬼，当前药剂数量：{submittedPotions.Count}");

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

