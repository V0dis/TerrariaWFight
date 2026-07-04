using System;
using System.Collections;
using UnityEngine;

public class Vampirism : Attacker
{
    [SerializeField] private float _damagePerSecond = 10;
    [SerializeField] private float _skillDuration;
    [SerializeField] private float _skillCooldown;
    
    public event Action<float> IsGetHealthPoints;
    public event Action IsSkillEnded;
    
    private Coroutine _attackCoroutine;
    private Coroutine _delayCoroutine;
    private float _damagePerTick;
    
    public float CurrentTime { get; private set; }
    public bool IsActive => _attackCoroutine != null;
    public bool IsDelay => _delayCoroutine != null;
    public float SkillDuration => _skillDuration;
    public float SkillCooldown => _skillCooldown;
    

    public void TryUseVampirism()
    {
        if (IsActive || IsDelay)
            return;
            
        _attackCoroutine = StartCoroutine(UseVampirism());
    }

    private IEnumerator UseVampirism()
    {
        float damagePerTick;
        
        CurrentTime = 0;
        
        while (_skillDuration > CurrentTime)
        { 
            CurrentTime += Time.deltaTime;
            damagePerTick = Time.deltaTime * _damagePerSecond;
            
            Health health = TryGetHealthOpponents();

            if (health != null)
                IsGetHealthPoints?.Invoke(health.TakeDamage(damagePerTick));
            
            yield return null;
        }
        
        IsSkillEnded?.Invoke();
        
        _delayCoroutine = StartCoroutine(Delay());
        _attackCoroutine = null;
    }
    
    private IEnumerator Delay()
    { 
        CurrentTime = 0;
        
        while (_skillCooldown > CurrentTime)
        { 
            CurrentTime += Time.deltaTime;

            yield return null;
        }
        
        _delayCoroutine = null;
    }
}