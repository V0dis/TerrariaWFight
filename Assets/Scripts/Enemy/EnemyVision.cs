using System.Collections;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [SerializeField] private float _defaultViewDistance = 10f;
    [SerializeField] private float _chaseViewDistance = 10f;
    [SerializeField] private float _coneAngle = 60f;
    [SerializeField] private float _checkPeriod = 0.1f;
    [SerializeField] private LayerMask _obstacleMask;
    [SerializeField] private bool _IsDrawGizmos = true;
    
    private CircleCollider2D _visionTrigger;
    private float _halfAngle;
    private Player _player;
    private RaycastHit2D _hit;
    private float _viewDistance;
    private float _sqrChaseViewDistance;

    public bool IsPlayerInSight { get; private set; } = false;

    public Player Player => _player;
    
    private void Awake()
    {
        _viewDistance = _defaultViewDistance;
        _sqrChaseViewDistance = _chaseViewDistance * _chaseViewDistance;
        _halfAngle = _coneAngle / 2f;
    }

    private void Start()
    {
        _visionTrigger = gameObject.AddComponent<CircleCollider2D>();
        _visionTrigger.radius = _defaultViewDistance;
        _visionTrigger.isTrigger = true;
        _visionTrigger.offset = GetCenter(transform) - transform.position;
        
        StartCoroutine(CheckPlayer(_checkPeriod));
    }

    public void CheckImpact()
    {
        IsPlayerInSight = true;
    }
    

    public IEnumerator CheckPlayer(float delay)
    {
        var wait = new WaitForSeconds(delay);

        while (enabled)
        {
            if (IsPLayerInCone(_player) && CanSeePlayer(_player))
            {
                _viewDistance = _chaseViewDistance;
                IsPlayerInSight = true;
            }

            if (CanSeePlayer(_player) == false || IsPlayerInRange == false)
            {
                _viewDistance = _defaultViewDistance;
                IsPlayerInSight = false;
            }
            
            yield return wait;
        }
    }

    private bool IsPlayerInRange =>
            (_player.transform.position - transform.position).sqrMagnitude <= _sqrChaseViewDistance;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player))
        {
            if (_player == null) 
                _player = player;
        }
    }
    
    private bool IsPLayerInCone(Player player)
    {
        if (player == null) 
            return false;
        
        Vector3 directonToPlayer = GetCenter(_player.transform) - GetCenter(transform);
        
        float cosAnglePlayer = Vector3.Dot(transform.right, directonToPlayer.normalized);
        float cosHalfCone = Mathf.Cos(_halfAngle * Mathf.Deg2Rad);
        
        return cosAnglePlayer >= cosHalfCone;
    }

    private bool CanSeePlayer(Player player)
    {
        if (player == null)
            return false;
        
        Vector3 center = GetCenter(transform);

        _hit = Physics2D.Raycast
        (
            center,
            GetCenter(player.transform) - center,
            _viewDistance,
            _obstacleMask
        );
        
        return _hit.transform == player.transform;
    }

    private Vector3 GetCenter(Transform transform)
    {
        if (transform.TryGetComponent(out Collider2D collider))
            return collider.bounds.center;

        return transform.position;
    }

    private void OnDrawGizmos()
    {
        if (_IsDrawGizmos == false) 
            return;
        
        DrawSphere(GetCenter(transform));

        if (IsPlayerInSight)
            DrawPlayerDirection();
        else
            DrawCone(GetCenter(transform));
    }

    private void DrawSphere(Vector3 center)
    {
        Gizmos.color = Color.gray;
        
        if (IsPlayerInSight)
            Gizmos.DrawWireSphere(center, _chaseViewDistance);
        else
            Gizmos.DrawWireSphere(center, _defaultViewDistance);
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
        Gizmos.DrawLine(center, center + corner * _defaultViewDistance);
    }
    
    private void DrawPlayerDirection()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(GetCenter(transform), _hit.point);
    }
}