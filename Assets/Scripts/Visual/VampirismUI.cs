using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class VampirismUI : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Vampirism _vampirism;

    Coroutine _coroutine;
    
    private void OnEnable()
    {
        _vampirism.CurrentTimeChanging += SetSliderValue;
    }

    private void Start()
    { 
        _slider.value = 1;
    }

    private void OnDisable()
    {
        _vampirism.CurrentTimeChanging -= SetSliderValue;
    }

    private void SetSliderValue(float previousTime)
    { 
        if (_coroutine != null)
            StopCoroutine(_coroutine);
        
        if (_vampirism.State == VampirismState.Active)
            SetActive(previousTime);
        if (_vampirism.State == VampirismState.Delay)
            SetDelay(previousTime);
    }

    private void SetDelay(float _)
    {
        float startValue = 0;
        float endValue = 1;
        
        _coroutine = StartCoroutine(ChangeSliderValue(startValue, endValue, _vampirism.SkillCooldown));
    }
    
    private void SetActive(float previousTime)
    {
        float startValue = 1 - previousTime / _vampirism.SkillDuration;
        float endValue = 1 - _vampirism.CurrentTime / _vampirism.SkillDuration;
        
        _coroutine = StartCoroutine(ChangeSliderValue(startValue, endValue, _vampirism.CurrentTick));
    }

    private IEnumerator ChangeSliderValue(float currentValue, float targetValue, float duration)
    {
        float elapsedTime = 0;
        
        while (elapsedTime <= duration)
        {
            elapsedTime += Time.deltaTime;

            _slider.value = Mathf.Lerp(currentValue, targetValue, elapsedTime / duration);

            yield return null;
        }
    }
}