using UnityEngine;

public class Cauldron : AbstractInteractables
{
    protected override void Awake()
    {
        base.Awake();
        // ���Գ�ʼ������״̬�����ʵ�
    }

    public override void OnInteraction()
    {
        Debug.Log("�������ҩ��");
        InventoryManager.Instance.ToggleInventory();  // ������ʾ

    }
}
