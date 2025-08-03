using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CraftingManager : MonoBehaviour
{
    public List<ItemDropSlot> craftingSlots;  // �ĸ��ϳɲ�
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

        //���� InventoryItem
        InventoryItem newItem = new InventoryItem(
            name: combinedName,
            iconSprite: itemsToCombine[0].icon,
            col: combinedColor,
            cooldown: maxCooldown,
            haunted: isHaunted,
            qty: 1
        );

        //�ύҩˮ�� PotionSubmitPanel
        PotionSubmitPanel.Instance.SubmitPotion(newItem);

        // ��ʾԤ��ͼ�꣨��ѡ��
        /*if (resultPreviewIcon != null)
        {
            resultPreviewIcon.sprite = newItem.icon;
            resultPreviewIcon.color = combinedColor;
            resultPreviewIcon.enabled = true;
        }*/

        // ��ղ�λ
        foreach (var slot in craftingSlots)
        {
            slot.ClearCraftSlot();
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
