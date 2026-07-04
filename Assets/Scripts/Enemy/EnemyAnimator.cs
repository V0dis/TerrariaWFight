using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimator : MonoBehaviour
{
    private static readonly int RunHash = Animator.StringToHash("Run");
    private static readonly int AttackHash1 = Animator.StringToHash("Attack1");
    private static readonly int AttackHash2 = Animator.StringToHash("Attack2");
    private static readonly int AttackHash3 = Animator.StringToHash("Attack3");
    private static readonly int DeadHash = Animator.StringToHash("Death");

    [SerializeField] private Animator _animator;
    
    private List<int> _attackHashes;

    private void Awake()
    {
        _attackHashes = new List<int>
        {
            AttackHash1, 
            AttackHash2, 
            AttackHash3
        };
    }

    public void SetMoving()
    {
        _animator.SetBool(RunHash, true);
    }

    public void SetIdle()
    {
        _animator.SetBool(RunHash, false);
    }

    public void SetMeleeAttack()
    {
        _animator.SetTrigger(_attackHashes[Random.Range(0, _attackHashes.Count)]);
    }

    public void SetDead()
    {
        _animator.SetTrigger(DeadHash);
    }
}
