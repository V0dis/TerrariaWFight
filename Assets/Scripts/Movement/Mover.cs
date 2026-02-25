using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Mover : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    public Vector2 Velocity =>
        _rigidbody != null ? _rigidbody.linearVelocity : Vector2.zero;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        if (_rigidbody)
            _rigidbody.freezeRotation = true;
    }

    public void Move(Vector2 direction)
    {
        if (_rigidbody)
            _rigidbody.linearVelocity = new Vector2(direction.x, _rigidbody.linearVelocity.y);
    }
}