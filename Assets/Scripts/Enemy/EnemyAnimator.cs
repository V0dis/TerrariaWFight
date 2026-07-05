using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimator : MonoBehaviour
{
    private static readonly int s_runHash = Animator.StringToHash("Run");
    private static readonly int s_attackHash1 = Animator.StringToHash("Attack1");
    private static readonly int s_attackHash2 = Animator.StringToHash("Attack2");
    private static readonly int s_attackHash3 = Animator.StringToHash("Attack3");
    private static readonly int s_deadHash = Animator.StringToHash("Death");

    [SerializeField] private Animator _animator;
    
    private List<int> _attackHashes;

    private void Awake()
    {
        _attackHashes = new List<int>
        {
            s_attackHash1, 
            s_attackHash2, 
            s_attackHash3
        };
    }

    public void SetMoving()
    {
        _animator.SetBool(s_runHash, true);
    }

    public void SetIdle()
    {
        _animator.SetBool(s_runHash, false);
    }

    public void SetMeleeAttack()
    {
        _animator.SetTrigger(_attackHashes[Random.Range(0, _attackHashes.Count)]);
    }

    public void SetDead()
    {
        _animator.SetTrigger(s_deadHash);
    }
}
