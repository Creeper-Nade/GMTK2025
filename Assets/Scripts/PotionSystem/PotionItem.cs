using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PotionItem : MonoBehaviour, IPointerClickHandler, IHauntAction
{
    public Image icon;
    public string potionID;
    public GameObject GameObject => gameObject;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetPotion(Sprite sprite, string id)
    {
        icon.sprite = sprite;
        potionID = id;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log($"�鿴ҩ����{potionID}");
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log($"����ҩ����{potionID}");
            Destroy(gameObject);
        }
    }

    //λ���������
    public void Haunt()
    {
        Vector2 randomOffset = new Vector2(Random.Range(-100f, 100f), Random.Range(-50f, 50f));
        rectTransform.anchoredPosition += randomOffset;
        Debug.Log($"ҩ�� {potionID} ���ֹ��ƶ�����λ�ã�");
    }

    public void ExitHaunt()
    {
        Debug.Log($"ҩ�� {potionID} �ָ�����״̬");
        // ����ʵ�ֻ�ԭλ���߼�
    }
}
