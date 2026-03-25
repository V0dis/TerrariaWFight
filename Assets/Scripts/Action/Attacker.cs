using UnityEngine;

public class Attacker : MonoBehaviour
{
    [SerializeField] private float _attackDelay = 0.5f;
    [SerializeField] private float _damage = 10;
    [SerializeField] private float _attackRange;
    [SerializeField] private int _maxTargetsForAttack = 2;

    private LayerMask _opponentLayer;
    private Collider2D[] _hitsBuffer;
    private float _nextAttackTime;

    public bool CanAttack => Time.time >= _nextAttackTime;

    public float AttackRange => _attackRange;

    public void Initialize(LayerMask opponentLayer)
    {
        _opponentLayer = opponentLayer;
        _hitsBuffer = new Collider2D[_maxTargetsForAttack];
    }

    public void TryAttack()
    {
        if (CanAttack == false)
            return;

        _nextAttackTime = Time.time + _attackDelay;

        int hitCount = Physics2D.OverlapCircleNonAlloc(transform.position, _attackRange, _hitsBuffer, _opponentLayer);

        for (int i = 0; i < hitCount; i++)
        {
            var hit = _hitsBuffer[i];
            
            if (hit.TryGetComponent(out Health health) && health.IsAlive && hit.isTrigger == false)
            {
                health.TakeDamage(_damage);
            }
        }
    }
}
