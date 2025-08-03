using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;  // ����LINQ������

public class CraftingManager : MonoBehaviour
{
    public List<ItemDropSlot> craftingSlots;  // �ĸ��ϳɲ�

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

        // ͳ����Ʒ����������
        Dictionary<string, int> nameCount = new Dictionary<string, int>();
        foreach (var item in itemsToCombine)
        {
            if (nameCount.ContainsKey(item.itemName))
                nameCount[item.itemName]++;
            else
                nameCount[item.itemName] = 1;
        }

        // ������ĸ��������
        var sortedNames = nameCount.Keys.OrderBy(name => name).ToList();

        // ƴ�Ӻϳ�����
        string combinedName = "�ϳ���(";
        for (int i = 0; i < sortedNames.Count; i++)
        {
            string name = sortedNames[i];
            int count = nameCount[name];
            combinedName += $"{name}��{count}";
            if (i < sortedNames.Count - 1)
                combinedName += " + ";
        }
        combinedName += ")";

        // �ϲ���ȴʱ�䣨���ֵ��
        float maxCooldown = itemsToCombine.Max(item => item.cooldownTime);

        // �Ƿ��ֹ�
        bool isHaunted = itemsToCombine.Any(item => item.isHaunted);

        // ������ InventoryItem
        InventoryItem newItem = new InventoryItem(
            name: combinedName,
            iconSprite: itemsToCombine[0].icon,
            col: combinedColor,
            cooldown: maxCooldown,
            haunted: isHaunted,
            qty: 1
        );

        // �ύҩˮ
        PotionSubmitPanel.Instance.SubmitPotion(newItem);

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
