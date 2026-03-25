using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [SerializeField] private float _checkRadius = 0.1f;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Transform _checkPoint;

    public bool IsGrounded { get; private set; }

    private void Awake()
    {
        if (_checkPoint == null)
            _checkPoint = transform;
    }

    private void Update()
    {
        Check();
    }

    public void Check()
    {
        Debug.DrawRay(_checkPoint.position, Vector2.down * _checkRadius, Color.red);

        Collider2D hit = Physics2D.OverlapCircle(_checkPoint.position, _checkRadius, _groundLayer);
        IsGrounded = hit != null;
    }
}
