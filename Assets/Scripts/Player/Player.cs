using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Player : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private UserInput _userInput;
    [SerializeField] private Mover _mover;
    [SerializeField] private Jumper _jumper;
    [SerializeField] private CoinsCollector _coinsCollector;
    [SerializeField] private HealthPeekUper _healthPeekUper;
    [SerializeField] private Wallet _wallet;
    [SerializeField] private Rotator _rotator;
    [SerializeField] private PlayerAnimator _animator;
    [SerializeField] private GroundChecker _groundChecker;
    [SerializeField] private Attacker _attacker;
    
    [Header("Settings")]
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private bool _isHealableOnMaxHealth = false;
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _jumpForce = 15f;
    
    [Header("Attck Settings")]
    [SerializeField] private float _attackRange = 1f;
    [SerializeField] private LayerMask _opponentLayer;
    [SerializeField] private float _attackDamage = 1f;
    [SerializeField] private float _attackDelay = 0.5f;

    private float _attackTimer;

    private void Awake()
    {
        _attacker.Initialize( _attackDamage, _attackRange, _opponentLayer);
        _health.Initialize(_maxHealth);
    }

    private void OnEnable()
    {
        _coinsCollector.CoinPickedUp += _wallet.AddCoins;
        _healthPeekUper.HealthPackPickedUp += TryHeal;
        _health.IsDead += SetDead;
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
        _coinsCollector.CoinPickedUp -= _wallet.AddCoins;
        _healthPeekUper.HealthPackPickedUp -= TryHeal;
        _health.IsDead -= SetDead;
    }

    private void TryAttack()
    {
        if (_userInput.GetMouseClick() && Time.time - _attackTimer > _attackDelay)
        {
            _attackTimer = Time.time;
            
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

    private void SetDead()
    {
        _animator.SetDead();
    }

    private void TryHeal(HealthPack healthPack)
    {
        if (_health.IsFullHealth == false || _isHealableOnMaxHealth)
        {
            _healthPeekUper.PickUp(healthPack);
            _health.Heal(healthPack.Value);
        }
    }
}