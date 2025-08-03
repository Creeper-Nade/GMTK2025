using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingControl : Singleton<PostProcessingControl>
{
    //ui
    [SerializeField] private CanvasGroup GhostFace;
    //post processing
    private Volume _postProcessVolume;
    [SerializeField] private float _pulseDuration = 2f; // Time for film grain pulse
    public Vignette vignette;

    private ChromaticAberration _chromaticAberration;
    private FilmGrain _grain;
    protected override void Awake()
    {
        base.Awake();
        _postProcessVolume = GetComponent<Volume>();
        // Get effect settings
        _postProcessVolume.profile.TryGet(out _chromaticAberration);
        _postProcessVolume.profile.TryGet(out _grain);
        _postProcessVolume.profile.TryGet(out vignette);

        // Initialize effects
        _grain.intensity.value = 0;
        _chromaticAberration.intensity.value = 0;
        vignette.intensity.value = 0;
    }

    // Call this method when the player fails a quest
    public void TriggerFailureEffect()
    {
        
        // Permanent Chromatic Aberration increase
        StartCoroutine(IncreaseChromaticAberration());
        
        // Temporary Film Grain pulse
        StartCoroutine(PulseFilmGrain());
    }

    private IEnumerator IncreaseChromaticAberration()
    {
        float targetIntensity = GlobalDataManager.Instance._Failure / 3f; // Scale: 0, 0.33, 0.66, 1
        float startIntensity = _chromaticAberration.intensity.value;
        float elapsed = 0f;
        float duration = 1f; // Time to ramp up

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            _chromaticAberration.intensity.value = Mathf.Lerp(
                startIntensity, 
                targetIntensity, 
                elapsed / duration
            );
            yield return null;
        }
    }

    private IEnumerator PulseFilmGrain()
    {
        // Fade grain IN (0 → 1)
        float elapsed = 0f;
        while (elapsed < _pulseDuration / 2)
        {
            elapsed += Time.deltaTime;
            _grain.intensity.value = Mathf.Lerp(0, 1, elapsed / (_pulseDuration / 2));
            GhostFace.alpha= Mathf.Lerp(0, 0.2f, elapsed / (_pulseDuration / 2));
            yield return null;
        }

        // Fade grain OUT (1 → 0)
        elapsed = 0f;
        while (elapsed < _pulseDuration / 2)
        {
            elapsed += Time.deltaTime;
            _grain.intensity.value = Mathf.Lerp(1, 0, elapsed / (_pulseDuration / 2));
            GhostFace.alpha= Mathf.Lerp(0.2f, 0, elapsed / (_pulseDuration / 2));
            yield return null;
        }

        _grain.intensity.value = 0; // Ensure reset
    }
}
