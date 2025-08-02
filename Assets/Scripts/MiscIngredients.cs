using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MiscIngredients : AbstractInteractables, IHauntAction
{
    [Header("Properties")]
    [SerializeField] private string NormalName;
    [SerializeField] private List<string> WrongNames;
    [SerializeField] private Sprite Item_Sprite;
    [SerializeField] private Color color;
    [SerializeField] private float Cooldown;
    //Haunt variales
    public GameObject GameObject => gameObject;
    private bool is_haunted=false;
    public bool Is_Haunted => is_haunted;
    private enum Haunt_Types
    {
        none,
        vibrate,
        WrongDisplay
    }
    private Haunt_Types _haunt_Type;

    private void OnEnable() {
        if (is_haunted && _haunt_Type==Haunt_Types.vibrate)
        {
            StartCoroutine(Vibrate());
        }
    }

    private IEnumerator Vibrate()
    {
        float speed = 10.0f;
        float intensity = 0.1f;
        float timer = 0f;
        float MaximumTime = 0.2f;
        while (timer < MaximumTime)
        {
            timer += Time.deltaTime;
            transform.localPosition = intensity * new Vector2(
                Mathf.PerlinNoise(speed * Time.time, 1),
                Mathf.PerlinNoise(speed * Time.time, 2)
            );
            yield return null;
        }
    }

    public override void OnInteraction()
    {
        if (_haunt_Type == Haunt_Types.WrongDisplay)
        {
            Debug.Log("Give item with its properties messed up");
            SetHauntedItemProperty();

        }
        else
        {
            Debug.Log(gameObject+ "give ingredient");
            InventoryItem item = new InventoryItem(
                name: NormalName,
                iconSprite: Item_Sprite,
                col: color,
                cooldown: Cooldown,
                haunted: is_haunted,
                qty: 1
            );
             // 添加到背包
            InventoryManager.Instance.AddItem(item);
        }    
        //implement logic of giving
    }

    private void SetHauntedItemProperty()
    {
        string wrongName = WrongNames[Random.Range(0, WrongNames.Count)]; ;
        float wrongCD = Random.Range(0.0f, Cooldown - 0.1f);
        wrongCD = Mathf.Round(wrongCD * 10.0f) * 0.1f;
        InventoryItem item = new InventoryItem(
                name: wrongName,
                iconSprite: Item_Sprite,
                col: color,
                cooldown: wrongCD,
                haunted: is_haunted,
                qty: 1
            );
         // 添加到背包
            InventoryManager.Instance.AddItem(item);
    }
    public void ExitHaunt()
    {
        is_haunted = false;
        _haunt_Type = Haunt_Types.none;
    }

    public void Haunt()
    {
        int index = Random.Range(0, 2);
        Debug.Log("Haunt ingredients");
        //int index = 0;
        is_haunted = true;
        //Debug.Log(gameObject + "is ahunted");
        switch (index)
        {
            case 0:
                _haunt_Type = Haunt_Types.vibrate;
                break;
            case 1:
                //Debug.Log("Species change");
                _haunt_Type = Haunt_Types.WrongDisplay;
                
                break;

        }
    }

    

}
