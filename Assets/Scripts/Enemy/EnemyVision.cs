using System.Collections;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [SerializeField] private float _patrolViewDistance = 10f;
    [SerializeField] private float _chaseViewDistance = 15f;
    [SerializeField] private float _coneAngle = 60f;
    [SerializeField] private float _checkPeriod = 2f;
    [SerializeField] private LayerMask _obstacleMask;
    [SerializeField] private bool _isDrawGizmos = true;

    private LayerMask _playerMask;
    private Transform _target;
    private WaitForSeconds _wait;
    private float _halfAngle;
    
    private Vector3 _enemyCenter;
    private Vector3 _playerCenter;
    private Vector3 _direction;
    private int _layerMask;
    
    public bool IsPlayerInSight { get; private set; }

    public void Initialize(LayerMask playerMask)
    {
        _halfAngle = _coneAngle / 2f;
        _wait = new WaitForSeconds(_checkPeriod);
        StartCoroutine(CheckPlayer(_checkPeriod));
        _playerMask = playerMask;
    }

    public IEnumerator CheckPlayer(float delay)
    {
        while (enabled)
        {
            _enemyCenter = GetCenter(transform);

            if (_target != null)
            {
                _playerCenter = GetCenter(_target);
                _direction = (_playerCenter - _enemyCenter).normalized;
            }
    
            FindTargetInRange();
            
            if (IsPlayerInSight)
            {
                if (CanSeePlayer() == false || _target == null) 
                    IsPlayerInSight = false;
            }
            else
            {
                if (IsPlayerInCone() && CanSeePlayer())
                    IsPlayerInSight = true;
            }

            yield return _wait;
        }
    }

    public bool TryGetPlayerPosition(out Transform position)
    {
        if (IsPlayerInSight && _target != null)
        {
            position = _target;
            return true;
        }

        position = null;
        return false;
    }

    public void CheckForThreat()
    {
        FindTargetInRange();

        if (_target != null)
        {
            IsPlayerInSight = true;
        }
    }

    private void FindTargetInRange()
    {
        var collider = Physics2D.OverlapCircle(_enemyCenter, GetCurrentViewDistance(), _playerMask);

        if (collider != null)
            _target = collider.transform;
        else
            _target = null;
    }

    private float GetCurrentViewDistance()
    {
        return IsPlayerInSight
            ? _chaseViewDistance
            : _patrolViewDistance;
    }

    private bool IsPlayerInCone()
    {
        if (_target == null)
            return false;
        

        float cosAnglePlayer = Vector3.Dot(transform.right, _direction.normalized);
        float cosHalfCone = Mathf.Cos(_halfAngle * Mathf.Deg2Rad);
        
        return cosAnglePlayer >= cosHalfCone;
    }

    private bool CanSeePlayer()
    {
        if (_target != null)
        {
            var hit = Physics2D.Raycast(_enemyCenter, _direction, GetCurrentViewDistance(), _obstacleMask | _playerMask);
            
            if (hit.collider != null)
            {
                return _target == hit.collider.transform;
            }
        }
        
        return false;
    }

    private Vector3 GetCenter(Transform other)
    {
        if (other.TryGetComponent(out Collider2D collider))
            return collider.bounds.center;

        return other.position;
    }

    private void OnDrawGizmos()
    {
        if (_isDrawGizmos == false)
            return;
        
        DrawSphere(_enemyCenter);

        if (IsPlayerInSight && _target != null)
            DrawPlayerDirection();
        else
            DrawCone(_enemyCenter);
    }

    private void DrawSphere(Vector3 center)
    {
        Gizmos.color = Color.gray;

        if (IsPlayerInSight)
            Gizmos.DrawWireSphere(center, _chaseViewDistance);
        else
            Gizmos.DrawWireSphere(center, _patrolViewDistance);
    }

    private void DrawCone(Vector3 center)
    {
        Gizmos.color = Color.yellow;

        DrawCorner(center, _halfAngle);
        DrawCorner(center, -_halfAngle);
    }

    private void DrawCorner(Vector3 center, float angle)
    {
        Vector3 corner = Quaternion.Euler(0, 0, angle) * transform.right;
        Gizmos.DrawLine(center, center + corner * _patrolViewDistance);
    }

    private void DrawPlayerDirection()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(_enemyCenter, GetCenter(_target));
    }
}