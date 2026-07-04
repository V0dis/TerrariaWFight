using UnityEngine;

public class Attacker : MonoBehaviour
{
    [SerializeField] private float _attackRange;
    [SerializeField] private int _maxTargetsForAttack;
    [SerializeField] private LayerMask _opponentLayer;
    
    private Collider2D[] _hitsBuffer;

    public float AttackRange => _attackRange;

    public void Initialize(LayerMask opponentLayer)
    {
        _opponentLayer = opponentLayer;
        _hitsBuffer = new Collider2D[_maxTargetsForAttack];
    }

    protected Health TryGetHealthOpponents()
    {
        int hitCount = Physics2D.OverlapCircleNonAlloc(transform.position, _attackRange, _hitsBuffer, _opponentLayer);

        for (int i = 0; i < hitCount; i++)
        {
            var hit = _hitsBuffer[i];
            
            if (hit.TryGetComponent(out Health health) && health.IsAlive && hit.isTrigger == false)
            {
                return health;
            }
        }
        
        return null;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        //Gizmos.DrawWireSphere(transform.position, AttackRange);
    }
}