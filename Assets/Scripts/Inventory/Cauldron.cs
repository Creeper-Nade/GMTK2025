using UnityEngine;

public class Cauldron : AbstractInteractables
{
    protected override void Awake()
    {
        base.Awake();
        // 可以初始化锅的状态、材质等
    }

    public override void OnInteraction()
    {
        Debug.Log("点击了炼药锅");
        InventoryManager.Instance.ToggleInventory();  // 调用显示

    }
}
