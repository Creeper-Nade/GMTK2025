using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class AbstractInteractables : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Renderer _material_renderer;
    private int _OutlineActivate = Shader.PropertyToID("_Outline");
    private int _OutlineThickness = Shader.PropertyToID("_OutlineThickness");
    protected virtual void Awake()
    {
        _material_renderer = GetComponent<Renderer>();
    }
    public virtual void Curse()
    {
        //implement logic inside for curse mechanic
        //spawn particle system
        //initiate timer
        //set the status of this gameobject to Cursed
    }
    //implement unique interaction and the method to banish curse
    public abstract void OnInteraction();

    public void OnPointerClick(PointerEventData eventData)
    {
        //insert logic for removing curse
        //run unqiue logic、
        _material_renderer.material.SetInt(_OutlineActivate, 1);
        StartCoroutine(ClickEffectRoutine());
        OnInteraction();
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
