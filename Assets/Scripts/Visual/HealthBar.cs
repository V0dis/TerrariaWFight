using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private Slider _slider;
    [SerializeField, Min(0)] private float _timeSlider = 0.5f;
    
    private float _backupSliderValue;
    private Coroutine _animationSlider;

    private float CurrentSliderValue => _health.CurrentHealth / _health.MaxHealth;

    public void OnEnable()
    {
        _health.IsGettingHeal += SetValue;
        _health.IsGettingHit += SetValue;
    }

    private void Start()
    {
        SetValue();
    }
    
    private void OnDisable()
    {
        if (_health != null)
        {
            _health.IsGettingHeal -= SetValue;
            _health.IsGettingHit -= SetValue;
        }
    }

    private void SetValue()
    {
        if (_animationSlider != null)
            StopCoroutine(_animationSlider);
        
        _backupSliderValue = _slider.value;
        _animationSlider = StartCoroutine(ChangeBarSlider(_timeSlider, _slider, _backupSliderValue));
    }

    private IEnumerator ChangeBarSlider(float time, Slider slider, float backupValue)
    {
        float elapsedTime = 0;
        
        while (elapsedTime <= time)
        {
            elapsedTime += Time.deltaTime;
            
            slider.value = Mathf.Lerp(backupValue, CurrentSliderValue, elapsedTime / time);
            
            yield return null;
        }
        
        _slider.value = CurrentSliderValue;
    }
}