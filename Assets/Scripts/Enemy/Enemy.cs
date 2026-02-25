using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Mover _mover;
    [SerializeField] private Route _route;
    [SerializeField] private Rotator _rotator;
    [SerializeField] private EnemyAnimator _animator;
    [SerializeField] private EnemyVision _vision;
    [SerializeField] private Health _health;
    [SerializeField] private Flasher _flasher;
    [SerializeField] private Attacker _attacker;
    
    [Header("Settings")]
    [SerializeField] private float _healthPoints = 100f;
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _chaseSpeed = 3f;
    [SerializeField] private float _rangeToChange = 0.5f;
    [SerializeField] private bool _isDeleteOnDeath;
    
    [Header("Attack settings")]
    [SerializeField] private int _damage = 1;
    [SerializeField] private float _attackRange = 0.5f;
    [SerializeField] private LayerMask _opponentLayer;
    [SerializeField] private float _attackDelay = 2f;
    
    private Transform _currentWaypoint;
    private Vector2 _moveDirection;
    private float _sqrRange;
    private float _sqrAttackRange;
    private bool _isMoving;
    private float _attackTimer;
    private float _deathTimer = 1f;
    
    private bool IsWaypointReached =>
        (_currentWaypoint.position - transform.position).sqrMagnitude < _sqrRange;

    private bool IsPlayerClose =>
        (_vision.Player.transform.position - transform.position).sqrMagnitude < _sqrAttackRange;

    private void Awake()
    {
        if (_route == null || _mover == null)
            return;

        _route.Initialize();
        _health.Initialize(_healthPoints);
        _attacker.Initialize(_damage, _attackRange, _opponentLayer);
        
        _sqrRange = _rangeToChange * _rangeToChange;
        _sqrAttackRange = _attackRange * _attackRange;
        
        _currentWaypoint = _route.GetWaypoint();
    }

    private void OnEnable()
    {
        _health.IsGettingHit += _vision.CheckImpact;
        _health.IsDead += Dead;
    }

    private void Update()
    {
        if (_health.IsAlive == false)
            return;
        
        if (_vision.IsPlayerInSight)
        {
            if (IsPlayerClose)
            {
                TryAttack();
                StopMoving();
                _rotator.SetDirection(_vision.Player.transform.position.x - transform.position.x);
            }
            else
            {
                MoveToPlayer();
            }
        }
        else if (_currentWaypoint == null)
        {
            StopMoving();
        }
        else if (IsWaypointReached)
        {
            SwitchToNextWaypoint();
        }
        else
        {
            MoveToWaypoint();
        }
    }

    private void OnDisable()
    {
        _health.IsGettingHit -= _vision.CheckImpact;
        _health.IsDead -= Dead;
    }

    private void TryAttack()
    {
        if (Time.time - _attackTimer > _attackDelay)
        {
            _attackTimer = Time.time;
            
            _attacker.TryAttack();
            _animator.SetAttack();
        }
    }

    private void SwitchToNextWaypoint() =>
        _currentWaypoint = _route.GetWaypoint();

    private void StopMoving()
    {
        if (_isMoving)
        {
            _isMoving = false;
            _animator.SetIdle();
        }
    }

    private void MoveToPlayer()
    {
        _moveDirection = (_vision.Player.transform.position - transform.position).normalized;
        _mover.Move(_moveDirection * _chaseSpeed);
        
        Move();
    }

    private void MoveToWaypoint()
    {
        _moveDirection = (_currentWaypoint.position - transform.position).normalized;
        _mover.Move(_moveDirection * _speed);
        
        Move();
    }

    private void Move()
    {
        _isMoving = true;
        _rotator.SetDirection(_moveDirection.x);
        _animator.SetMoving();
    }

    private void Dead()
    {
        _animator.SetDead();
        
        GetComponent<Rigidbody2D>().simulated = false;
        GetComponent<Collider2D>().enabled = false;
        
        if (_isDeleteOnDeath)
            Destroy(gameObject, _deathTimer);
    }
}
