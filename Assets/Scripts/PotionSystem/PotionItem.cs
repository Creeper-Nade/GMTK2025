using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PotionItem : MonoBehaviour, IPointerClickHandler, IHauntAction
{
    public Image icon;
    public string potionID;

    public Color potionColor;
    public float cooldownTime;
    public bool isHaunted;
    bool IHauntAction.Is_Haunted => isHaunted;

    private RectTransform rectTransform;
    public GameObject GameObject => gameObject;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetPotion(Sprite sprite, string id, Color color, float cooldown, bool haunted)
    {
        icon.sprite = sprite;
        potionID = id;
        potionColor = color;
        cooldownTime = cooldown;
        isHaunted = haunted;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log($"查看药剂：{potionID}, 冷却时间：{cooldownTime}s");
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log($"丢弃药剂：{potionID}");
            Destroy(gameObject);
        }
    }

    public void Haunt()
    {
        if (!isHaunted) return;
        Vector2 randomOffset = new Vector2(Random.Range(-100f, 100f), Random.Range(-50f, 50f));
        rectTransform.anchoredPosition += randomOffset;
        Debug.Log($"药剂 {potionID} 被闹鬼，移动到新位置！");
    }

    public void ExitHaunt()
    {
        Debug.Log($"药剂 {potionID} 恢复正常状态");
        // 可加还原位置逻辑
    }
}
