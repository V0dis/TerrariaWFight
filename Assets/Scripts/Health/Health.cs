using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    private float _maxValue;
    private float _currentValue;
    
    public event Action IsGettingHit;
    public event Action IsGettingHeal;
    public event Action IsDead;
    
    public float CurrentValue => _currentValue;
    public float MaxValue => _maxValue;
    public bool IsAlive => _currentValue > 0;

    public void Initialize(float maxValue)
    {
        _maxValue = maxValue;
        _currentValue = maxValue;
    }

    public float TakeDamage(float damage)
    {
        if (IsAlive)
        {
            float takenDamage = _currentValue < damage ? _currentValue : damage;
            
            _currentValue = Mathf.Max(0, _currentValue - damage);
            
            IsGettingHit?.Invoke();

            if (_currentValue <= 0)
                IsDead?.Invoke();
            
            return takenDamage;
        }
        
        return 0;
    }

    public void TakeHeal(float amount)
    {
        if (IsAlive)
        {
            _currentValue = Mathf.Min(_maxValue, _currentValue + amount);
            IsGettingHeal?.Invoke();
        }
    }
}
