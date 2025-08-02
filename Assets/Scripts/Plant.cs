using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;


/// <summary>
/// ҩ����ɫö�٣����ڷ���ϳɻ���ʾ
/// </summary>
    public enum HerbColor
    {
        Red,
        Blue,
        Green,
        Yellow,
        Purple,
    }


public class Plant : AbstractInteractables, IHauntAction
{
    #region Growth variables
    [SerializeField] private float _Growth_Time;
    //[SerializeField] private Animator animatro;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite _no_sprite;
    [SerializeField] private Sprite _sprout_sprite;
    [SerializeField] private Sprite _seedling_sprite;
    [SerializeField] private Sprite _plant_sprite;

    public float Current_Time;
    


    [SerializeField] private HerbColor herbColor;
    public HerbColor HerbColor => herbColor;


    public enum Growth_Stages
    {
        none,
        sprout,
        seedling,
        plant
    }
    public Growth_Stages stage;
    #endregion

    #region Haunt Varibles
    private enum Haunt_Types
    {
        none,
        SpeciesChange,
        NoCD
    }
    private bool is_haunted=false;
    private Haunt_Types _haunt_Type;
    [SerializeField] private List<GameObject> _GhostPlants;
    private GameObject GhostPlant;

    public GameObject GameObject => gameObject;

    bool IHauntAction.Is_Haunted => is_haunted;
    #endregion
    protected override void Awake()
    {
        base.Awake();
        
        _haunt_Type = Haunt_Types.none;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        if (is_haunted)
        {
            //Debug.Log("OnEnabel: " + is_haunted);
            switch (_haunt_Type)
            {
                case Haunt_Types.SpeciesChange:
                    gameObject.SetActive(false);
                    break;
                case Haunt_Types.NoCD:
                    stage = Growth_Stages.plant;
                    //mild short shake logic
                    break;

            }

        }
        switch (stage)
        {
            case Growth_Stages.none:
                spriteRenderer.sprite = _no_sprite;
                break;
            case Growth_Stages.sprout:
                spriteRenderer.sprite = _sprout_sprite;
                break;
            case Growth_Stages.seedling:
                spriteRenderer.sprite = _seedling_sprite;
                break;
            case Growth_Stages.plant:
                spriteRenderer.sprite = _plant_sprite;
                break;

        }
    }
    public override void OnInteraction()
    {
        if (stage == Growth_Stages.plant)
        {
            Debug.Log("Give Item");
            _StageNoneBehavior();
            //implement the logic for giving item here
            //InventoryManager.Instance.AddMaterialByColor(HerbColor);
        }
    }

    public void GrowthElapse()
    {
        Current_Time = Mathf.Min(Current_Time + Time.deltaTime, _Growth_Time);
        //Too bad i cant use switch here, since GrowthTime isn't constant

        if (stage == Growth_Stages.plant) return;
        if (Current_Time < _Growth_Time / 3 && stage != Growth_Stages.none)
        {
            _StageNoneBehavior();
        }
        else if (Current_Time >= _Growth_Time / 3 && Current_Time < _Growth_Time / 3 * 2 && stage != Growth_Stages.sprout)
        {
            _StageSproutBehavior();
        }
        else if (Current_Time >= _Growth_Time / 3 * 2 && Current_Time < _Growth_Time && stage != Growth_Stages.seedling)
        {
            _StageSeedlingBehavior();
        }
        else if (Current_Time >= _Growth_Time && stage != Growth_Stages.plant)
        {
            _StagePlantBehavior();
        }
    }
    private void _StageNoneBehavior()
    {
        stage = Growth_Stages.none;
        Current_Time = 0;
        //Debug.Log("nope");
        spriteRenderer.sprite = _no_sprite;
        //implement logic for animation
    }
    private void _StageSproutBehavior()
    {
        stage = Growth_Stages.sprout;
        //animatro.SetTrigger("trig");
        spriteRenderer.sprite = _sprout_sprite;
        //Debug.Log("sprout");
        //implement logic for animation
    }
    private void _StageSeedlingBehavior()
    {
        stage = Growth_Stages.seedling;
        //Debug.Log("seedling");
        spriteRenderer.sprite = _seedling_sprite;
        //implement logic for animation
    }
    private void _StagePlantBehavior()
    {
        stage = Growth_Stages.plant;
        spriteRenderer.sprite = _plant_sprite;
        //Debug.Log("plant");
        //implement logic for animation
    }

    public void Haunt()
    {
        int index = Random.Range(0, 2);
        //int index = 1;
        is_haunted = true;
        //Debug.Log(gameObject + "is ahunted");
        switch (index)
        {
            case 0:
                _haunt_Type = Haunt_Types.NoCD;
                break;
            case 1:
                //Debug.Log("Species change");
                _haunt_Type = Haunt_Types.SpeciesChange;
                int plantListIndex = Random.Range(0, _GhostPlants.Count);

                GhostPlant = ObjectPoolManager.Instance.SpawnObject(_GhostPlants[plantListIndex], transform.parent, Quaternion.identity);
                GhostPlant.transform.position = transform.position;
                Plant plant = GhostPlant.GetComponent<Plant>();
                GlobalDataManager.Instance.Plants.Add(plant);
                plant.is_haunted = true;
                plant.stage = stage;
                plant.Current_Time = Current_Time;
                break;

        }
    }

    public void ExitHaunt()
    {
        is_haunted = false;

        //Debug.Log("OnExit haunt: "+is_haunted);
        switch (_haunt_Type)
        {
            case Haunt_Types.SpeciesChange:
                //logic for removing haunted plant
                //Debug.Log(gameObject +"Ghost: "+ GhostPlant);
                ObjectPoolManager.Instance.ReturnObjectToPool(GhostPlant);
                GlobalDataManager.Instance.Plants.Remove(GhostPlant.GetComponent<Plant>());
                break;
            case Haunt_Types.NoCD:
                _StageNoneBehavior();
                break;
        }
        _haunt_Type = Haunt_Types.none;
        gameObject.SetActive(true);
    }
}
