using System.Collections;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [SerializeField] private float _checkRadius = 0.1f;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Transform _checkPoint;
    [SerializeField] private float _checkInterval = 0.1f;

    private WaitForSeconds _waitInterval;

    public bool IsGrounded { get; private set; }

    public void Awake()
    {
        if (_checkPoint == null)
            _checkPoint = transform;
        
        _waitInterval = new WaitForSeconds(_checkInterval);
        StartCoroutine(CheckGround());
    }

    private IEnumerator CheckGround()
    {
        while (enabled)
        {
            Check();
            yield return _waitInterval;
        }
    }

    public void Check()
    {
        Debug.DrawRay(_checkPoint.position, Vector2.down * _checkRadius, Color.red);

        Collider2D hit = Physics2D.OverlapCircle(_checkPoint.position, _checkRadius, _groundLayer);
        IsGrounded = hit != null;
    }
}
