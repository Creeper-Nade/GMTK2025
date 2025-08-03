// CraftingManager.cs
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CraftingManager : MonoBehaviour
{
    public List<ItemDropSlot> craftingSlots;  // �ĸ��ϳɲ�
    public Image resultPreviewIcon;

    public GameObject potionPrefab;  // Ԥ���壬�������� PotionItem
    public Transform potionParent;   // �������ɵ� PotionItem

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
            Debug.Log("û����Ʒ���ںϳɡ�");
            return;
        }

        // �ϲ���ɫ
        Color combinedColor = CombineColors(itemsToCombine);

        // �ϲ�����
        string combinedName = "�ϳ���(";
        for (int i = 0; i < itemsToCombine.Count; i++)
        {
            combinedName += itemsToCombine[i].itemName;
            if (i < itemsToCombine.Count - 1) combinedName += "+";
        }
        combinedName += ")";

        // �ϲ���ȴʱ�䣨���ֵ��
        float maxCooldown = 0f;
        foreach (var item in itemsToCombine)
        {
            if (item.cooldownTime > maxCooldown)
                maxCooldown = item.cooldownTime;
        }

        // �Ƿ��ֹ�
        bool isHaunted = false;
        foreach (var item in itemsToCombine)
        {
            if (item.isHaunted)
            {
                isHaunted = true;
                break;
            }
        }

        // ���� PotionItem
        GameObject newPotionGO = Instantiate(potionPrefab, potionParent);
        PotionItem potionComponent = newPotionGO.GetComponent<PotionItem>();
        if (potionComponent != null)
        {
            potionComponent.SetPotion(
                sprite: itemsToCombine[0].icon,  // �ɶ���ͼ��
                id: combinedName,
                color: combinedColor,
                cooldown: maxCooldown,
                haunted: isHaunted
            );
        }

        // ��ʾԤ��ͼ��
        if (resultPreviewIcon != null)
        {
            resultPreviewIcon.sprite = itemsToCombine[0].icon;
            resultPreviewIcon.color = combinedColor;
            resultPreviewIcon.enabled = true;
        }

        // ��ղ�λ
        foreach (var slot in craftingSlots)
        {
            slot.ClearSlot();
        }

        Debug.Log("�ϳɳɹ���" + combinedName);
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
