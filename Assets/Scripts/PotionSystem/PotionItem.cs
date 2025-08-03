using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PotionItem : MonoBehaviour, IHauntAction, IPointerEnterHandler, IPointerExitHandler
{
    public Image icon;
    public string potionID;
    public Color potionColor;
    public float cooldownTime;
    public bool isHaunted;

    // ��������
    public GameObject submitButton;
    public GameObject destroyButton;

    public GameObject GameObject => throw new System.NotImplementedException();

    public bool Is_Haunted => throw new System.NotImplementedException();

    public void SetPotion(Sprite sprite, string id, Color color, float cooldown, bool haunted)
    {
        icon.sprite = sprite;
        potionID = id;
        potionColor = color;
        cooldownTime = cooldown;
        isHaunted = haunted;

        HideButtons();
    }

    public void Haunt()
    {
        if (!isHaunted) return;

        Transform parent = transform.parent;
        int selfIndex = transform.GetSiblingIndex();
        int siblingCount = parent.childCount;
        if (siblingCount <= 1) return;

        int otherIndex = Random.Range(0, siblingCount);
        while (otherIndex == selfIndex)
            otherIndex = Random.Range(0, siblingCount);

        Transform other = parent.GetChild(otherIndex);
        other.SetSiblingIndex(selfIndex);
        transform.SetSiblingIndex(otherIndex);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)parent);
    }

    // ��������ʾ��ť
    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowButtons();
    }

    // ����Ƴ����ذ�ť
    public void OnPointerExit(PointerEventData eventData)
    {
        HideButtons();
    }

    private void ShowButtons()
    {
        if (submitButton != null) submitButton.SetActive(true);
        if (destroyButton != null) destroyButton.SetActive(true);
    }

    private void HideButtons()
    {
        if (submitButton != null) submitButton.SetActive(false);
        if (destroyButton != null) destroyButton.SetActive(false);
    }

    public void OnSubmitClicked()
    {
        Debug.Log($"�ύ��{potionID}");
        // TODO: ��������ύ�߼�
    }

    public void OnDestroyClicked()
    {
        Debug.Log($"���٣�{potionID}");
        Destroy(gameObject);
    }

    public void ExitHaunt()
    {
        throw new System.NotImplementedException();
    }
}

