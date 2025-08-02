using System.Collections.Generic;
using UnityEngine;

public class PotionSubmitPanel : MonoBehaviour
{
    public Transform slotContainer;          // �ύ���� UI ������ʹ�� Horizontal/Vertical Layout Group��
    public GameObject potionPrefab;          // ҩ�� prefab������ PotionItem.cs��

    private List<GameObject> submittedPotions = new List<GameObject>();

    public void SubmitPotion(Sprite icon, string potionID)
    {
        GameObject newPotion = Instantiate(potionPrefab, slotContainer);
        PotionItem potionItem = newPotion.GetComponent<PotionItem>();
        potionItem.SetPotion(icon, potionID);
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
}

