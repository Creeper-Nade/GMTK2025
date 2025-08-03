using UnityEngine;
using UnityEngine.UI;

public class OrderManager : MonoBehaviour
{
    [SerializeField] private Sprite[] orderSprites;  // �� Inspector ������������ͼ
    [SerializeField] private Image orderDisplayImage; // Inspector ������ UI Image ���������ʾ������ͼ

    private int currentOrderIndex = -1;

    void Start()
    {
        RefreshOrder();
    }
    public void SetOrderVisible(bool visible)
    {
        Debug.Log("���� UI ��ʾ״̬��" + visible);
        if (orderDisplayImage != null)
            orderDisplayImage.gameObject.SetActive(visible);
    }
    public void RefreshOrder()
    {
        if (orderSprites.Length == 0 || orderDisplayImage == null)
        {
            Debug.LogWarning("������ͼ����Ϊ�ջ�δ����չʾ Image");
            return;
        }

        int newIndex;
        do
        {
            newIndex = Random.Range(0, orderSprites.Length);
        } while (newIndex == currentOrderIndex && orderSprites.Length > 1); // ��֤������ϴ�һ��

        currentOrderIndex = newIndex;
        orderDisplayImage.sprite = orderSprites[currentOrderIndex];
    }

    public void SubmitPotion()
    {
        // ���������������ύ�߼��������ж�ҩ���Ƿ�ƥ�䶩����

        Debug.Log("ҩ���ύ�ɹ���ˢ�¶�����");
        RefreshOrder();
    }
}
