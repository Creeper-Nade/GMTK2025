using System.Collections.Generic;
using UnityEngine;

public class PotionSubmitPanel : MonoBehaviour
{
    public Transform slotContainer;          // 提交区的 UI 容器（使用 Horizontal/Vertical Layout Group）
    public GameObject potionPrefab;          // 药剂 prefab（挂着 PotionItem.cs）

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

