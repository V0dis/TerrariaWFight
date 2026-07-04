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
    [SerializeField] private MeleeAttacker _attacker;
    [SerializeField] private Flasher _flasher;
    [SerializeField] private Vampirism _vampirism;
    [SerializeField] private VampirismVisualArea _vampirismVisualArea;

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
        _vampirism.Initialize(_opponentLayer);
        _vampirismVisualArea.Initialize(_vampirism.AttackRange);
    }

    private void OnEnable()
    {
        _itemCollector.CoinPickedUp += _wallet.AddCoins;
        _itemCollector.HealthPackPickedUp += _health.Heal;
        _health.IsGettingHit += _flasher.DamageFlash;
        _health.IsGettingHeal += _flasher.HealFlash;
        _health.IsDead += _animator.SetDead;
        _vampirism.IsGetHealthPoints += _health.Heal;
        _vampirism.IsSkillEnded += _vampirismVisualArea.Hide;
    }

    private void Update()
    {
        if (_health.IsAlive)
        {
            TryMelleAttack();
            TryUseVampirism();
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
        _vampirism.IsGetHealthPoints -= _health.Heal;
        _vampirism.IsSkillEnded -= _vampirismVisualArea.Hide;
    }

    private void TryMelleAttack()
    {
        if (_userInput.GetAttackInput() && _attacker.IsAttacking == false)
        {
            _attacker.TryMeleeAttack();
            _animator.SetMeleeAttack();
        }
    }

    private void TryUseVampirism()
    {
        if (_userInput.GetVampirismSkillInput() && _vampirism.IsActive == false && _vampirism.IsDelay == false)
        {
            _vampirism.TryUseVampirism();
            _vampirismVisualArea.Show();
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