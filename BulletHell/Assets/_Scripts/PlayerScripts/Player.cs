using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerScripts
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private GameObject _shootingLine;
        [SerializeField] private Color _bulletColor;
        [SerializeField] private AudioSource _audio;
        [SerializeField] private AudioClip _collect;
        [SerializeField] private AudioClip _shoot;
        [SerializeField] private GameObject _lStick;
        [SerializeField] private GameObject _rStick;


        private Vector2 _direction;
        private Vector2 _lastStickVl;
        private SpriteRenderer _spriteRenderer;
        private Rigidbody2D _rigidbody2D;
        private int _bullets;
        private float _shootingSpeed = 20;
        private bool _shootingState;
        private bool _phoneControls;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (Input.touches.Length > 0)
            {
                _phoneControls = true;
                _lStick.SetActive(true);
                _rStick.SetActive(true);
            }

            if (!_phoneControls) return;

            var direction = Gamepad.current.leftStick.ReadValue();
            _direction = direction;


            var rVal = Gamepad.current.rightStick.ReadValue();
            if (rVal != Vector2.zero && _bullets > 0)
            {
                var shootingDirection = rVal;
                _lastStickVl = rVal;
                float zRotation = Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg;
                _shootingLine.transform.rotation = Quaternion.Euler(0, 0, zRotation - 90);
                _shootingLine.SetActive(true);
                _shootingState = true;
            }
            else if (rVal == Vector2.zero)
            {
                if (_shootingState)
                {
                    Shoot(_lastStickVl);
                    _shootingLine.SetActive(false);
                    _shootingState = false;
                }
            }
            else
            {
                _shootingLine.SetActive(false);
            }
        }

        public void ResetShootingSpeed()
        {
            _shootingSpeed = 20;
        }

        private void FixedUpdate()
        {
            _rigidbody2D.velocity = new Vector2(_direction.x, _direction.y) * _speed;
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            _phoneControls = false;
            _direction = context.ReadValue<Vector2>();
        }

        public void OnButtonClick(InputAction.CallbackContext context)
        {
            _phoneControls = false;
            if (context.performed) Shoot();
        }

        private void Shoot()
        {
            if (_bullets == 0) return;
            var position = transform.position;
            var projectile = Instantiate(_bulletPrefab, position, Quaternion.identity).GetComponent<Projectile>();
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            projectile.Initialize(mousePosition - position, _shootingSpeed);
            _spriteRenderer.color = Color.white;
            _bullets--;
            _audio.PlayOneShot(_shoot);
        }

        private void Shoot(Vector3 direction)
        {
            if (_bullets == 0) return;
            var position = transform.position;
            var projectile = Instantiate(_bulletPrefab, position, Quaternion.identity).GetComponent<Projectile>();
            projectile.Initialize(direction, _shootingSpeed);
            _spriteRenderer.color = Color.white;
            _bullets--;
            _audio.PlayOneShot(_shoot);
        }

        private void Collect()
        {
            if (_bullets > 0) return;
            _spriteRenderer.color = _bulletColor;
            _bullets++;
            _audio.PlayOneShot(_collect);
        }

        private void OnEnable()
        {
            FoodCollectable.OnCollect += Collect;
            HitCounter.OnValue2Reached += ImproveShootingSpeed;
        }

        private void ImproveShootingSpeed()
        {
            if (_shootingSpeed >= 40) return;
            _shootingSpeed += 2;
        }


        private void OnDisable()
        {
            FoodCollectable.OnCollect -= Collect;
            HitCounter.OnValue2Reached -= ImproveShootingSpeed;
        }
    }
}