using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _speed;
    private Rigidbody2D _rigidbody2D;
    private Vector2 _direction;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _rigidbody2D.velocity = _direction.normalized * _speed;
        var rotation = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotation + 90);
    }

    public void Initialize(Vector2 direction)
    {
        _direction = direction;
    }

    public void Initialize(Vector2 direction, float shootingSpeed)
    {
        _direction = direction;
        _speed = shootingSpeed;
    }
}