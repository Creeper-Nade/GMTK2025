using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GlobalDataManager : Singleton<GlobalDataManager>
{
    public RoomBase GreenHouse;
    public List<Plant> Plants;
    [SerializeField] private List<RoomBase> _RoomList;
    private List<AbstractInteractables> _Interactables = new();
    private List<IHauntAction> _Hauntables = new();

    private Dictionary<GameObject, RoomBase> InteractableRoomPair = new();

    //this list is to handle all the problemetic hauntables that remains because of being active, should normally be empty.
    private List<IHauntAction> _RemainedHauntables = new();
    [SerializeField] private int GhostNumber = 2;
    [SerializeField] private int CurseGhostNumber = 1;
    [SerializeField] private float _HauntCooldown = 8;
    [SerializeField] private float _CurseCooldown = 5;
    [Range(0.0f, 1.0f)]
    [SerializeField] private float _HauntProbability;
    [Range(0.0f, 1.0f)]
    [SerializeField] private float _CurseProbability;
    private IHauntAction[] _HauntedObjects = new IHauntAction[999];
    private AbstractInteractables[] _CursedObjects = new AbstractInteractables[999];
    private bool _HauntCalled = false;
    private bool _CurseCalled = false;
    private bool _VignetteFadeInCalled = false;
    private bool _VignetteFadeOutCalled = false;

    public int _Failure;

    protected override void Awake()
    {
        base.Awake();
        //get all plants from greenhouse
        Plant[] plants = GreenHouse.GetComponentsInChildren<Plant>();
        for (int i = 0; i < plants.Length; i++)
        {
            Plants.Add(plants[i]);
        }
        //get all room and pair the interactable objects with the parent room
        foreach (RoomBase room in _RoomList)
        {
            AbstractInteractables[] children = room.GetComponentsInChildren<AbstractInteractables>();
            for (int i = 0; i < children.Length; i++)
            {
                //Debug.Log(children[i]);
                _Interactables.Add(children[i]);
                InteractableRoomPair.Add(children[i].gameObject, room);
            }
        }
        //initialize plant status
        foreach (Plant plant in Plants)
        {
            plant.Current_Time = 0;
            plant.stage = Plant.Growth_Stages.none;
        }

        foreach (AbstractInteractables interactables in _Interactables)
        {
            if (interactables.GetComponent<IHauntAction>() != null)
            {
                _Hauntables.Add(interactables.GetComponent<IHauntAction>());
                //Debug.Log(interactables);
            }

        }
        // Debug.Log("Interactables size: " + _Interactables.Count);
        //Debug.Log("Hauntable size: " + _Hauntables.Count);
    }

    // Update is called once per frame
    void Update()
    {
        //simulate Update in disabled plants
        foreach (Plant plant in Plants)
        {
            plant.GrowthElapse();
            //Debug.Log(plant+ ": "+plant.is_haunted);
        }
        //simulate update for cursed objects
        for (int i = 0; i < CurseGhostNumber; i++)
        {
            if (_CursedObjects[i] != null && _CursedObjects[i].is_cursed)
            {
                if(!_VignetteFadeInCalled)
                StartCoroutine(VignetteFadeIn(_CursedObjects[i]._CurseDuration));
                _CursedObjects[i].CurseElapse();
            }
        }
        if (_CursedObjects != null && PostProcessingControl.Instance.vignette.intensity.value > 0)
        {
        // Filter out nulls and check for any cursed objects
        bool anyCursed = _CursedObjects
            .Where(obj => obj != null)  // Filter out null objects
            .Any(obj => obj.is_cursed); // Check if any remaining object is cursed

        if (!anyCursed && !_VignetteFadeOutCalled)
        {
            StartCoroutine(VignetteFadeOut());
        }
        }
        
        if (!_CurseCalled)
            StartCoroutine(CallCurse());
        if (!_HauntCalled)
            StartCoroutine(CallHaunt());

        //process the problemetic hauntables
        if (_RemainedHauntables.Count == 0) return;

        IHauntAction[] localHauntArray = _RemainedHauntables.ToArray();
        for (int i = 0; i < localHauntArray.Length; i++)
        {
            if (!InteractableRoomPair[localHauntArray[i].GameObject].gameObject.activeInHierarchy)
            {
                //Debug.Log(_HauntedObjects[i] + "exits haunt from disposal list");
                localHauntArray[i].ExitHaunt();
                _RemainedHauntables.Remove(localHauntArray[i]);
            }
        }
    }
    private IEnumerator VignetteFadeIn(float duration)
    {
        _VignetteFadeInCalled = true;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
             PostProcessingControl.Instance.vignette.intensity.value = Mathf.Lerp(0, 0.5f, elapsed / duration);
             bool anyCursed = _CursedObjects
            .Where(obj => obj != null)  // Filter out null objects
            .Any(obj => obj.is_cursed); // Check if any remaining object is cursed

            if (!anyCursed)
            {
                break;
            }
            yield return null;
        }
    }
    private IEnumerator VignetteFadeOut()
    {
        _VignetteFadeOutCalled = true;
        float duration = 0.5f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            PostProcessingControl.Instance.vignette.intensity.value = Mathf.Lerp(0.5f, 0f, elapsed / duration);
            yield return null;
        }
        _VignetteFadeOutCalled = false;
        _VignetteFadeInCalled = false;
    }

    private IEnumerator CallCurse()
    {
        _CurseCalled = true;
        //we need to ensure there is only one gameobject assigned to each ghost, or else the haunted gameobject might not be able to reset
        List<AbstractInteractables> Eligible = new();
        foreach (AbstractInteractables CurseAction in _Interactables)
        {
            if (!InteractableRoomPair[CurseAction.gameObject].gameObject.activeInHierarchy && !CurseAction.is_cursed)
            {
                //Debug.Log("Eligible list contains:" + hauntAction);
                IHauntAction haunt = CurseAction.GetComponent<IHauntAction>();
                if (haunt != null && !haunt.Is_Haunted)
                    Eligible.Add(CurseAction);
            }
        }
        Debug.Log("eligible curse number:" + Eligible.Count);
        //bind objects
        for (int i = 0; i < CurseGhostNumber; i++)
        {

            //Debug.Log("GhostNumber: " + i + "Binded object: " + _HauntedObjects[i]);
            if (_CursedObjects[i] == null || !_CursedObjects[i].is_cursed)
            {
                if (Random.value <= _CurseProbability && Eligible.Count > 0)
                {
                    AbstractInteractables obj = Eligible[Random.Range(0, Eligible.Count)];
                    //Debug.Log(obj + " is haunt");
                    _CursedObjects[i] = obj;
                    obj.Curse();
                    Eligible.Remove(obj);
                }
            }

        }
        yield return new WaitForSeconds(_CurseCooldown);
        _CurseCalled = false;
    }

    private IEnumerator CallHaunt()
    {
        _HauntCalled = true;
        //we need to ensure there is only one gameobject assigned to each ghost, or else the haunted gameobject might not be able to reset
        List<IHauntAction> Eligible = new();
        foreach (IHauntAction hauntAction in _Hauntables)
        {
            if (!InteractableRoomPair[hauntAction.GameObject].gameObject.activeInHierarchy && !hauntAction.Is_Haunted)
            {
                //Debug.Log("Eligible list contains:" + hauntAction);
                AbstractInteractables curse = hauntAction.GameObject.GetComponent<AbstractInteractables>();
                if (!curse.is_cursed)
                {
                    Eligible.Add(hauntAction);
                }

            }
        }
        //clean old object binded in pair
        for (int i = 0; i < GhostNumber; i++)
        {

            //Debug.Log("GhostNumber: " + i + "Binded object: " + _HauntedObjects[i]);
            if (_HauntedObjects[i] != null)
            {
                if (!InteractableRoomPair[_HauntedObjects[i].GameObject].gameObject.activeInHierarchy)
                {
                    //Debug.Log(_HauntedObjects[i] + "exits haunt directly");
                    _HauntedObjects[i].ExitHaunt();
                    _HauntedObjects[i] = null;
                }
                else
                {
                    //Debug.Log(_HauntedObjects[i] + "pending on disposal");
                    _RemainedHauntables.Add(_HauntedObjects[i]);
                    Eligible.Remove(_HauntedObjects[i]);
                    _HauntedObjects[i] = null;
                }
            }

        }
        //Rebind gameobjects
        for (int i = 0; i < GhostNumber; i++)
        {

            //Debug.Log("Attempt Haunt"+Eligible.Count);
            if (Random.value <= _HauntProbability && Eligible.Count > 0)
            {
                IHauntAction obj = Eligible[Random.Range(0, Eligible.Count)];
                //Debug.Log(obj + " is haunt");
                _HauntedObjects[i] = obj;
                obj.Haunt();
                Eligible.Remove(obj);
            }
        }
        yield return new WaitForSeconds(_HauntCooldown);
        _HauntCalled = false;
    }
    public void FailAction()
    {
        _Failure++;
        _Failure=Mathf.Clamp(_Failure, 0, 3);
        if (_Failure < 3)
        {
            PostProcessingControl.Instance.TriggerFailureEffect();
        }
    }
    private void LoseEffect()
    {

    }
}
