using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Player : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private UserInput _userInput;
    [SerializeField] private Mover _mover;
    [SerializeField] private Jumper _jumper;
    [SerializeField] private ItemCollector _itemCollector;
    [SerializeField] private Wallet _wallet;
    [SerializeField] private Rotator _rotator;
    [SerializeField] private PlayerAnimator _animator;
    [SerializeField] private GroundChecker _groundChecker;
    [SerializeField] private Attacker _attacker;
    [SerializeField] private Flasher _flasher;

    [Header("Settings")]
    [SerializeField] private float _maxHealth = 100f;

    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _jumpForce = 15f;
    [SerializeField] private LayerMask _opponentLayer;
    
    private Transform _transform;
 
    private void Awake()
    {
        _attacker.Initialize(_opponentLayer);
        _health.Initialize(_maxHealth);
    }

    private void OnEnable()
    {
        _itemCollector.CoinPickedUp += _wallet.AddCoins;
        _itemCollector.HealthPackPickedUp += _health.Heal;
        _health.IsGettingHit += _flasher.DamageFlash;
        _health.IsGettingHeal += _flasher.HealFlash;
        _health.IsDead += _animator.SetDead;
    }

    private void Update()
    {
        if (_health.IsAlive)
        {
            TryAttack();
            Move(_userInput.GetMoveInput().x);
            TryJump();
            RefreshAnimation();
        }
    }

    private void OnDisable()
    {
        _itemCollector.CoinPickedUp -= _wallet.AddCoins;
        _itemCollector.HealthPackPickedUp -= _health.Heal;
        _health.IsGettingHit -= _flasher.DamageFlash;
        _health.IsGettingHeal -= _flasher.HealFlash;
        _health.IsDead -= _animator.SetDead;
    }

    private void TryAttack()
    {
        if (_userInput.GetAttackInput() && _attacker.CanAttack)
        {
            _attacker.TryAttack();
            _animator.SetAttack();
        }
    }
    
    private void Move(float horizontalInput)
    {
        _mover.Move(new Vector2(horizontalInput * _speed, 0));
        _rotator.SetDirection(horizontalInput);
    }

    private void TryJump()
    {
        if (_userInput.GetJumpInput() && _groundChecker.IsGrounded)
        {
            _jumper.Jump(_jumpForce);
            _animator.SetJump();
        }
    }

    private void RefreshAnimation()
    {
        _animator.SetSpeed(Mathf.Abs(_mover.Velocity.x));
        _animator.SetGrounded(_groundChecker.IsGrounded);
        _animator.SetAirSpeedY(_mover.Velocity.y);
    }

    private void Deactivate()
    {
        if(TryGetComponent<Collider2D>(out var collider)) 
            collider.enabled = false;
    }
}