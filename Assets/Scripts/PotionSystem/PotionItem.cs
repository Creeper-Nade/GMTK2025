using UnityEngine;
using UnityEngine.UI;

public class PotionItem : MonoBehaviour, IHauntAction
{
    public Image icon;
    public string potionID;
    public Color potionColor;
    public float cooldownTime;
    public bool isHaunted;

    public GameObject GameObject => throw new System.NotImplementedException();

    public bool Is_Haunted => throw new System.NotImplementedException();

    public void SetPotion(Sprite sprite, string id, Color color, float cooldown, bool haunted)
    {
        icon.sprite = sprite;
        potionID = id;
        potionColor = color;
        cooldownTime = cooldown;
        isHaunted = haunted;
    }

    public void Haunt()
    {
        if (!isHaunted) return;

        Transform parent = transform.parent;
        int selfIndex = transform.GetSiblingIndex();
        int siblingCount = parent.childCount;

        if (siblingCount <= 1)
        {
            Debug.LogWarning("只有一个药剂，无法交换位置");
            return;
        }

        // 选择另一个药剂并交换位置
        int otherIndex = Random.Range(0, siblingCount);
        while (otherIndex == selfIndex)
            otherIndex = Random.Range(0, siblingCount);

        Transform other = parent.GetChild(otherIndex);
        other.SetSiblingIndex(selfIndex);
        transform.SetSiblingIndex(otherIndex);

        // 强制刷新 UI 排列
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)parent);

        Debug.Log($"[Haunt] {potionID} 与 {other.name} 互换位置");
    }

    public void ExitHaunt()
    {
        throw new System.NotImplementedException();
    }
}

