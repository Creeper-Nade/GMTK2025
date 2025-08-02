using UnityEngine;

public class CollectPlantsOnClick : MonoBehaviour
{
    private Plant plant;

    private void Awake()
    {
        plant = GetComponent<Plant>();
    }

    private void OnMouseDown()
    {
        if (plant == null)
        {
            Debug.LogWarning("�Ҳ��� Plant ���");
            return;
        }

        if (plant.stage != Plant.Growth_Stages.plant)
        {
            Debug.Log("ֲ����δ����");
            return;
        }

        // ��ӵ�����
        //InventoryManager.Instance.AddMaterialByColor(plant.HerbColor);
        //Debug.Log("�ɼ�ֲ���ɫΪ��" + plant.HerbColor);

        // ��������
        Destroy(gameObject);
    }
}