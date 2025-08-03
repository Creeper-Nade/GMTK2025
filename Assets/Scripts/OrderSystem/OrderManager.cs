using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderManager : MonoBehaviour
{
    [System.Serializable]
    public class OrderEntry
    {
        public Sprite orderSprite;
        public string orderID;  // ��ʽ���ϳ���(Duskmoth+Mandrake Root+...)
    }

    public List<OrderEntry> orders;              // ���п�ѡ����
    public Image orderDisplayImage;              // ��ʾ������ͼ�� UI Image
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
    }
}
