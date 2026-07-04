using System.Collections;
using UnityEngine;

public class MeleeAttacker : Attacker
{
    [SerializeField] private float _attackDelay = 1f;
    [SerializeField] private float _damage = 10;
    
    private Coroutine _delayCoroutine;
    
    public bool IsAttacking => _delayCoroutine != null;
    
    public void TryMeleeAttack()
    {
        if (IsAttacking == false)
        {
            _delayCoroutine = StartCoroutine(Attack());
        }
    }
    
    private IEnumerator Attack()
    {
        Health health = TryGetHealthOpponents();
        
        if (health != null) 
            health.TakeDamage(_damage);
        
        yield return new WaitForSeconds(_attackDelay);
        
        _delayCoroutine = null;
    }
}
