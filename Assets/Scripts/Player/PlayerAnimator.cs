using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    private const float MinSpeedForRun = 0.1f;

    private static readonly int s_runHash = Animator.StringToHash("Run");
    private static readonly int s_groundedHash = Animator.StringToHash("Grounded");
    private static readonly int s_airSpeedYHash = Animator.StringToHash("AirSpeedY");
    private static readonly int s_jumpHash = Animator.StringToHash("Jump");
    private static readonly int s_attackHash1 = Animator.StringToHash("Attack1");
    private static readonly int s_attackHash2 = Animator.StringToHash("Attack2");
    private static readonly int s_attackHash3 = Animator.StringToHash("Attack3");
    private static readonly int s_deadHash = Animator.StringToHash("Death");

    [SerializeField] private Animator _animator;
    
    private List<int> _attackHashes;

    public void Awake()
    {
        if (_animator == null)
            _animator = GetComponent<Animator>();

        _attackHashes = new List<int>
        {
            s_attackHash1,
            s_attackHash2,
            s_attackHash3
        };
    }

    public void SetSpeed(float speed)
    {
        _animator.SetBool(s_runHash, Mathf.Abs(speed) > MinSpeedForRun);
    }

    public void SetGrounded(bool isGrounded)
    {
        _animator.SetBool(s_groundedHash, isGrounded);
    }

    public void SetMeleeAttack()
    {
        _animator.SetTrigger(_attackHashes[Random.Range(0, _attackHashes.Count)]);
    }

    public void SetAirSpeedY(float velocityY)
    {
        _animator.SetFloat(s_airSpeedYHash, velocityY);
    }

    public void SetDead()
    {
        _animator.SetTrigger(s_deadHash);
    }

    public void SetJump()
    {
        _animator.SetTrigger(s_jumpHash);
    }
}
