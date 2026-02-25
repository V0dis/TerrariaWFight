using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    private float _maxHealth;
    private float _currentHealth;

    public Action IsDead;
    public Action IsGettingHit;
    
    public bool IsAlive => _currentHealth > 0;
    
    public bool IsFullHealth => Mathf.Approximately(_currentHealth, _maxHealth);

    public void Initialize(float maxHealth)
    {
        _maxHealth = _currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        IsGettingHit?.Invoke();
        
        if (IsAlive)
        {
            _currentHealth = Mathf.Max(0, _currentHealth - damage);
            
            if (TryGetComponent(out Flasher flash))
                flash.DamageFlash();
            
            if (_currentHealth <= 0)
                IsDead?.Invoke();
        }
    }

    public void Heal(float amount)
    {
        if (IsAlive)
            _currentHealth = Mathf.Min(_maxHealth, _currentHealth + amount);
        
        if (TryGetComponent(out Flasher flash)) 
            flash.HealFlash();
    }
}
