using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    private float _maxHealth;
    private float _currentHealth;
    
    public Action IsGettingHit;
    public Action IsGettingHeal;
    public Action IsDead;
    
    public bool IsAlive => _currentHealth > 0;
    
    public bool IsFullHealth => Mathf.Approximately(_currentHealth, _maxHealth);

    public void Initialize(float maxHealth)
    {
        _maxHealth = maxHealth;
        _currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (IsAlive)
        {
            _currentHealth = Mathf.Max(0, _currentHealth - damage);
            IsGettingHit?.Invoke();

            if (_currentHealth <= 0)
                IsDead?.Invoke();
        }
    }

    public void Heal(float amount)
    {
        if (IsAlive)
        {
            _currentHealth = Mathf.Min(_maxHealth, _currentHealth + amount);
            IsGettingHeal?.Invoke();
        }
    }
}
