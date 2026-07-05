using UnityEngine;

public class Enemy : MonoBehaviour
{
    private enum State
    {
        Patrol, Chase, Attack, Idle
    }

    [SerializeField] private Mover _mover;
    [SerializeField] private Route _route;
    [SerializeField] private Rotator _rotator;
    [SerializeField] private EnemyAnimator _animator;
    [SerializeField] private EnemyVision _vision;
    [SerializeField] private Health _health;
    [SerializeField] private MeleeAttacker _meleeAttacker;
    [SerializeField] private Flasher _flasher;

    [Header("Settings")]
    [SerializeField] private float _healthPoints = 100f;
    [SerializeField] private float _patrolSpeed = 2f;
    [SerializeField] private float _chaseSpeed = 3f;
    [SerializeField] private float _rangeToChangeWaypoint = 0.5f;
    [SerializeField] private bool _isDeleteOnDeath;
    [SerializeField] private LayerMask _opponentLayer;

    private Vector3 _playerPosition;
    private Transform _currentWaypoint;
    private float _sqrRangeToChangeWaypoint;
    private float _sqrAttackRange;
    private float _sqrDistanceToPlayer;
    private bool _isMoving;
    private float _deathTimer = 1f;

    private bool IsWaypointReached =>
        (_currentWaypoint.position - transform.position).sqrMagnitude < _sqrRangeToChangeWaypoint;

    private void Awake()
    {
        _route.Initialize();
        _health.Initialize(_healthPoints); 
        _vision.Initialize(_opponentLayer); 
        _meleeAttacker.Initialize(_opponentLayer);
        
        _sqrAttackRange = _meleeAttacker.AttackRange * _meleeAttacker.AttackRange; 
        _sqrRangeToChangeWaypoint = _rangeToChangeWaypoint * _rangeToChangeWaypoint;
        
        _currentWaypoint = _route.GetWaypoint();
    }

    private void OnEnable()
    {
        _health.IsGettingHit += _vision.CheckForThreat; 
        _health.IsGettingHit += _flasher.DamageFlash;
        _health.IsDead += Dead;
    }

    private void Update()
    {
        if (_health.IsAlive == false)
            return;
        
        UpdateState(GetCurrentState());
    }

    private void OnDisable()
    {
        _health.IsGettingHit -= _vision.CheckForThreat;
        _health.IsGettingHit -= _flasher.DamageFlash;
        _health.IsDead -= Dead;
    }

    private State GetCurrentState()
    {
        if (_vision.IsPlayerInSight && _vision.TryGetPlayerPosition(out var playerPosition))
        {
            _playerPosition = playerPosition.position;
            _sqrDistanceToPlayer = (_playerPosition - transform.position).sqrMagnitude;
            
            return (_sqrAttackRange >= _sqrDistanceToPlayer) 
                ? State.Attack 
                : State.Chase;
        }
        
        if (_currentWaypoint != null)
            return State.Patrol;
        
        return State.Idle;
    }

    private void UpdateState(State state)
    {
        switch (state)
        {
            case State.Attack:
                Attack();
                break;

            case State.Chase:
                Chase();
                break;

            case State.Patrol:
                Patrol();
                break;

            case State.Idle:
                StopMoving();
                break;
            
            default:
                StopMoving();
                break;
        }
    }

    private void Attack()
    {
        TryAttack();
        _rotator.TrySetDirection(_playerPosition.x - transform.position.x);
        StopMoving();
    }

    private void Chase()
    {
        Move(_playerPosition, _chaseSpeed); 
    }

    private void Patrol()
    {
        if (IsWaypointReached)
        {
            SwitchToNextWaypoint();
            
            if (_currentWaypoint == null)
            {
                StopMoving();
                return;
            }
        }
        
        Move(_currentWaypoint.position, _patrolSpeed);
    }

    private void TryAttack()
    {
        if (_meleeAttacker.IsAttacking == false)
        {
            _meleeAttacker.TryMeleeAttack();
            _animator.SetMeleeAttack();
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

    private void Move(Vector3 direction, float speed)
    {
        _isMoving = true;
        var correctVector = direction - transform.position;
        
        _mover.Move(correctVector.normalized * speed);
        _rotator.TrySetDirection(correctVector.x);
        _animator.SetMoving();
    }

    private void Dead()
    {
        _animator.SetDead();

        if (TryGetComponent(out Rigidbody2D rigidbody))
            rigidbody.simulated = false;

        if (TryGetComponent(out Collider2D collider))
            collider.enabled = false;

        if (_isDeleteOnDeath)
            Destroy(gameObject, _deathTimer);
    }
}
