<<<<<<< HEAD
using System.Collections.Generic;
=======
>>>>>>> main
using UnityEngine;
using UnityEngine.UI;

public class OrderManager : MonoBehaviour
{
<<<<<<< HEAD
    [System.Serializable]
    public class OrderEntry
    {
        public Sprite orderSprite;
        public string orderID; 
    }

    public List<OrderEntry> orders;          
    public Image orderDisplayImage;             
    public string CurrentOrderID { get; private set; }

    public static OrderManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        GenerateNewOrder();
    }

    public void GenerateNewOrder()
    {
        if (orders == null || orders.Count == 0)
        {
            Debug.LogWarning("�����б�Ϊ�գ�");
            return;
        }

        int index = Random.Range(0, orders.Count);
        OrderEntry selected = orders[index];

        CurrentOrderID = selected.orderID;
        orderDisplayImage.sprite = selected.orderSprite;

        Debug.Log("�¶������ɣ�" + CurrentOrderID);
    }

    public bool CheckOrderMatch(string potionID)
    {
        return potionID == CurrentOrderID;
=======
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
>>>>>>> main
    }
}
