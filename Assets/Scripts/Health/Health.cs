using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    private float _maxHealth;
    private float _currentHealth;
    
    public event Action IsGettingHit;
    public event Action IsGettingHeal;
    public event Action IsDead;
    
    public float CurrentHealth => _currentHealth;
    public float MaxHealth => _maxHealth;
    public bool IsAlive => _currentHealth > 0;

    public void Initialize(float maxHealth)
    {
        _maxHealth = maxHealth;
        _currentHealth = maxHealth;
    }

    public float TakeDamage(float damage)
    {
        if (IsAlive)
        {
            float takenDamage = _currentHealth < damage ? _currentHealth : damage;
            
            _currentHealth = Mathf.Max(0, _currentHealth - damage);
            
            IsGettingHit?.Invoke();

            if (_currentHealth <= 0)
                IsDead?.Invoke();
            
            return takenDamage;
        }
        
        return 0;
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
