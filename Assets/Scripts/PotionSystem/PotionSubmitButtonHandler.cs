using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PotionSubmitButtonHandler : MonoBehaviour
{
    public List<ItemDropSlot> craftingSlots; // ��������ĸ���λ

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
            Debug.Log("û����Ч��Ʒ�ύ��");
        }
    }
}
