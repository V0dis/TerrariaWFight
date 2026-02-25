using System.Collections;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    [SerializeField] private float _offsetX, _offsetY;
    [SerializeField] private bool _isDrawGizmos;
    
    private Vector3 _offset;
    private Vector3 _centerAttackArea;
    private float _damage;
    private float _attackRange;
    private LayerMask _opponentLayer;
    private Collider2D[] _hits;
    
    private float _timeDelay = 0.1f;

    public void Initialize(float attackDamage, float attackRange, LayerMask opponentLayer)
    {
        _damage = attackDamage;
        _attackRange = attackRange;
        _opponentLayer = opponentLayer;
        
        StartCoroutine(SetCenterAttackArea());
    }

    public void TryAttack()
    {
        _hits = Physics2D.OverlapCircleAll(GetCenter(transform) + _centerAttackArea, _attackRange, _opponentLayer);

        foreach (var hit in _hits)
        {
            if (hit.TryGetComponent(out Health health) && health.IsAlive && hit.isTrigger == false)
            {
                health.TakeDamage(_damage);
            }
        }

        _hits = null;
    }

    private Vector3 GetCenter(Transform transform)
    {
        if (TryGetComponent(out Collider2D collider))
            return collider.bounds.center;

        return transform.position;
    }

    private IEnumerator SetCenterAttackArea()
    {
        while (enabled)
        {
            _centerAttackArea = _offsetX * transform.right + _offsetY * transform.up;
            yield return new WaitForSeconds(_timeDelay);
        }
    }

    private void OnDrawGizmos()
    {
        if (_isDrawGizmos == false)
            return;
        
        Gizmos.color = Color.blueViolet;
        Gizmos.DrawWireSphere(GetCenter(transform) + _centerAttackArea, _attackRange);
    }
}
