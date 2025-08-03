using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderManager : MonoBehaviour
{
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
            Debug.LogWarning("订单列表为空！");
            return;
        }

        int index = Random.Range(0, orders.Count);
        OrderEntry selected = orders[index];

        CurrentOrderID = selected.orderID;
        orderDisplayImage.sprite = selected.orderSprite;

        Debug.Log("新订单生成：" + CurrentOrderID);
    }

    public bool CheckOrderMatch(string potionID)
    {
        return potionID == CurrentOrderID;
    }
}
