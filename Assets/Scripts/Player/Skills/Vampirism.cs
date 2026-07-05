using System;
using System.Collections;
using UnityEngine;

public enum VampirismState
{
    None, 
    Active, 
    Delay, 
    Ready
}

public class Vampirism : Attacker
{
    [SerializeField] private float _damagePerSecond = 10;
    [SerializeField] private float _skillDuration;
    [SerializeField] private float _skillCooldown;
    [SerializeField] private float _tickInterval = 0.1f;

    private float _damagePerTick;
    
    public event Action<float> GettingHealthPoints;
    public event Action SkillEnded;
    public event Action<float> CurrentTimeChanging;

    public float CurrentTime { get; private set; }
    public VampirismState State { get; private set; }
    public float CurrentTick { get; private set; }
    public float SkillDuration => _skillDuration;
    public float SkillCooldown => _skillCooldown;

    private void Start()
    {
        _damagePerTick = _damagePerSecond * _tickInterval;
        State = VampirismState.Ready;
    }

    public void TryUse()
    {
        if (State != VampirismState.Ready)
            return;
            
        StartCoroutine(Use());
    }

    private IEnumerator Use()
    {
        State = VampirismState.Active;
        
        CurrentTick = _tickInterval;
        CurrentTime = 0;
        
        var tick = new WaitForSeconds(CurrentTick);
        float previousTime;
        
        
        while (_skillDuration > CurrentTime)
        {
            previousTime = CurrentTime;

            if (_skillDuration - CurrentTime < CurrentTick)
            {
                CurrentTick = _skillDuration - CurrentTime;
                tick = new WaitForSeconds(CurrentTick);
            }
            
            CurrentTime += CurrentTick;
            
            CurrentTimeChanging?.Invoke(previousTime);
            
            Health health = TryGetHealthOpponents();

            if (health != null)
                GettingHealthPoints?.Invoke(health.TakeDamage(_damagePerTick));
            
            yield return tick;
        }
        
        SkillEnded?.Invoke();
        
        StartCoroutine(Delay());
    }
    
    private IEnumerator Delay()
    {
        State = VampirismState.Delay;
        
        CurrentTimeChanging?.Invoke(0);
        
        yield return new WaitForSeconds(_skillCooldown);
        
        State = VampirismState.Ready;
    }
}