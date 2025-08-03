using UnityEngine;
using System.Collections.Generic;  // 添加这个以支持 List

[System.Serializable]
public class InventoryItem
{
    public string itemName;
    public Sprite icon;
    public Color color;
    public float cooldownTime;
    public bool isHaunted;
    public int quantity = 1;

    //public List<string> tags;  

    public InventoryItem(string name, Sprite iconSprite, Color col, float cooldown, bool haunted, int qty = 1, List<string> itemTags = null)
    {
        itemName = name;
        icon = iconSprite;
        color = col;
        cooldownTime = cooldown;
        isHaunted = haunted;
        quantity = qty;
        //tags = itemTags != null ? new List<string>(itemTags) : new List<string>();  // 初始化为非空列表
    }
}
