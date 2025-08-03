using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class AbstractInteractables : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Renderer _material_renderer;
    private int _OutlineActivate = Shader.PropertyToID("_Outline");
    private int _OutlineThickness = Shader.PropertyToID("_OutlineThickness");
    private int _DistortionActivate = Shader.PropertyToID("_Distortion");
    [SerializeField] private GameObject CurseParticlePrefab;
    private GameObject _CurrentCurseParticleSystem;
    public float _CurseDuration = 9f;
    private float _CurrentCurseTime;

    public bool is_cursed = false;
    public enum CurseState
    {
        none,
        stage_1,
        stage_2,
        stage_3,
        stage_final

    }
    public CurseState curseState;
    protected virtual void Awake()
    {
        _material_renderer = GetComponent<Renderer>();
        curseState = CurseState.none;
    }
    protected virtual void OnEnable()
    {
        if (is_cursed && curseState == CurseState.stage_final)
        {
            FailCurse();
        }
    }
    protected virtual void OnDisable() {
        if (_CurrentCurseParticleSystem != null && !is_cursed)
        {
            // ObjectPoolManager.Instance.ReturnObjectToPool(_CurrentCurseParticleSystem, ObjectPoolManager.PoolType.ParticleSystems);
            GlobalDataManager.Instance.CallPoolCurseParticle(_CurrentCurseParticleSystem);
        }
    }
    private void FailCurse()
    {
        Debug.Log("fail for once");
        GlobalDataManager.Instance.FailAction();
        CleanCurse();
    }
    private void CleanCurse()
    {
        is_cursed = false;
        curseState = CurseState.none;
        _material_renderer.material.SetInt(_DistortionActivate, 0);
            //stop main smoke particle
        ParticleSystem particle = _CurrentCurseParticleSystem.GetComponent<ParticleSystem>();
        particle.Stop();
        StartCoroutine(ClearCurseParticle());
    }
    public virtual void Curse()
    {
        //implement logic inside for curse mechanic
        Debug.Log(gameObject + "Is cursed");
        curseState = CurseState.stage_1;
        _material_renderer.material.SetInt(_DistortionActivate, 1);
        _CurrentCurseParticleSystem = ObjectPoolManager.Instance.SpawnObject(CurseParticlePrefab, transform, Quaternion.identity, ObjectPoolManager.PoolType.ParticleSystems);
        for (int i = _CurrentCurseParticleSystem.transform.childCount - 1; i >= 0; i--)
        {
            _CurrentCurseParticleSystem.transform.GetChild(i).gameObject.SetActive(false);
        }
        is_cursed = true;
        //spawn particle system
        //initiate timer
        //set the status of this gameobject to Cursed
    }
    public void CurseElapse()
    {
        _CurrentCurseTime = Mathf.Min(_CurrentCurseTime + Time.deltaTime, _CurseDuration);

        if (curseState == CurseState.stage_final)
        {
            if (gameObject.activeInHierarchy)
            {
                FailCurse();
            }
            return;
        }

        if (_CurrentCurseTime >= _CurseDuration / 3 && _CurrentCurseTime < _CurseDuration / 3 * 2 && curseState != CurseState.stage_2)
            {
                curseState = CurseState.stage_2;
                _CurrentCurseParticleSystem.GetComponent<CursedParticleSystem>().GhostSmoke.SetActive(true);
            }
            else if (_CurrentCurseTime >= _CurseDuration / 3 * 2 && _CurrentCurseTime < _CurseDuration && curseState != CurseState.stage_3)
            {
                curseState = CurseState.stage_3;
                _CurrentCurseParticleSystem.GetComponent<CursedParticleSystem>().BloodMist.SetActive(true);
            }
            else if (_CurrentCurseTime >= _CurseDuration && curseState != CurseState.stage_final)
            {
                curseState = CurseState.stage_final;
            }
    }
    //implement unique interaction and the method to banish curse
    public abstract void OnInteraction();

    public void OnPointerClick(PointerEventData eventData)
    {
        //insert logic for removing curse
        //run unqiue logic、
        _material_renderer.material.SetInt(_OutlineActivate, 1);
        StartCoroutine(ClickEffectRoutine());
        //exit curse if clicked
        if (is_cursed)
        {
            CleanCurse();
        }
        else
        {
            OnInteraction();
        }    
    }
    private IEnumerator ClearCurseParticle()
    {
        yield return new WaitForSeconds(1.0f);
        ObjectPoolManager.Instance.ReturnObjectToPool(_CurrentCurseParticleSystem, ObjectPoolManager.PoolType.ParticleSystems);
        _CurrentCurseParticleSystem = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Enter Hover");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Exit Hover");
    }

    private IEnumerator ClickEffectRoutine()
    {
        const float fadeInDuration = 0.03f;
        const float fadeOutDuration = 0.07f;
        const float peakValue = 0.05f;

        // Track progress with local variables
        float timer = 0f;
        float currentValue = 0f;

        // Fade IN to white
        while (timer < fadeInDuration)
        {
            timer += Time.deltaTime;
            currentValue = Mathf.Lerp(0f, peakValue, timer / fadeInDuration);
            _material_renderer.material.SetFloat(_OutlineThickness, currentValue);
            yield return null;
        }

        // Fade OUT to normal
        timer = 0f;
        while (timer < fadeOutDuration)
        {
            timer += Time.deltaTime;
            currentValue = Mathf.Lerp(peakValue, 0f, timer / fadeOutDuration);
            _material_renderer.material.SetFloat(_OutlineThickness, currentValue);
            yield return null;
        }

        // Ensure final state
        _material_renderer.material.SetFloat(_OutlineThickness, 0f);
        _material_renderer.material.SetInt(_OutlineActivate, 0);
        
        //hitEffectCoroutine = null;
    }
}
