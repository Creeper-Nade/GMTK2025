using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public string itemName;
    public Sprite icon;
    public Color color;
    public float cooldownTime;
    public bool isHaunted;

    public int quantity = 1;

    public InventoryItem(string name, Sprite iconSprite, Color col, float cooldown, bool haunted, int qty = 1)
    {
        itemName = name;
        icon = iconSprite;
        color = col;
        cooldownTime = cooldown;
        isHaunted = haunted;
        quantity = qty;
    }
}
