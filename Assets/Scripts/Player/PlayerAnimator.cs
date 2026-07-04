using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    private const float MinSpeedForRun = 0.1f;

    private static readonly int RunHash = Animator.StringToHash("Run");
    private static readonly int GroundedHash = Animator.StringToHash("Grounded");
    private static readonly int AirSpeedYHash = Animator.StringToHash("AirSpeedY");
    private static readonly int JumpHash = Animator.StringToHash("Jump");
    private static readonly int AttackHash1 = Animator.StringToHash("Attack1");
    private static readonly int AttackHash2 = Animator.StringToHash("Attack2");
    private static readonly int AttackHash3 = Animator.StringToHash("Attack3");
    private static readonly int DeadHash = Animator.StringToHash("Death");

    [SerializeField] private Animator _animator;
    
    private List<int> _attackHashes;

    public void Awake()
    {
        if (_animator == null)
            _animator = GetComponent<Animator>();

        _attackHashes = new List<int>
        {
            AttackHash1,
            AttackHash2,
            AttackHash3
        };
    }

    public void SetSpeed(float speed)
    {
        _animator.SetBool(RunHash, Mathf.Abs(speed) > MinSpeedForRun);
    }

    public void SetGrounded(bool isGrounded)
    {
        _animator.SetBool(GroundedHash, isGrounded);
    }

    public void SetMeleeAttack()
    {
        _animator.SetTrigger(_attackHashes[Random.Range(0, _attackHashes.Count)]);
    }

    public void SetAirSpeedY(float velocityY)
    {
        _animator.SetFloat(AirSpeedYHash, velocityY);
    }

    public void SetDead()
    {
        _animator.SetTrigger(DeadHash);
    }

    public void SetJump()
    {
        _animator.SetTrigger(JumpHash);
    }
}
