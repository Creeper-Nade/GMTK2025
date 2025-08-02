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
            Debug.LogWarning("找不到 Plant 组件");
            return;
        }

        if (plant.stage != Plant.Growth_Stages.plant)
        {
            Debug.Log("植物尚未成熟");
            return;
        }

        // 添加到背包
        //InventoryManager.Instance.AddMaterialByColor(plant.HerbColor);
        //Debug.Log("采集植物，颜色为：" + plant.HerbColor);

        // 销毁自身
        Destroy(gameObject);
    }
}