using System;
using System.Collections;
using System.Collections.Generic;
using PlayerScripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _attackDelayMin;
        [SerializeField] private float _attackDelayMax;
        [SerializeField] private float _startAttackDelay;
        [SerializeField] private Projectile _enemyProjectile;

        private Rigidbody2D _rigidbody2D;
        private Vector2 _direction;
        private WaitForSeconds _delay;


        public delegate void EnemyEvent();

        public static event EnemyEvent OnDie;

        private void Awake()
        {
            _delay = new WaitForSeconds(_startAttackDelay);
            _rigidbody2D = GetComponent<Rigidbody2D>();
            var modX = Random.Range(-1, 1);
            var modY = Random.Range(-1, 1);


            modX = (modX >= 0) ? 1 : -1;
            modY = (modY >= 0) ? 1 : -1;

            _direction = new Vector2(Random.Range(0.3f, 1) * modX, Random.Range(0.3f, 1) * modY).normalized;
        }

        private void FixedUpdate()
        {
            _rigidbody2D.velocity = new Vector2(_direction.x, _direction.y) * _speed;
        }

        public void Redirect(int modX, int modY)
        {
            _direction = new Vector2(modX * _direction.x, modY * _direction.y);
        }

        public void Initialize(uint level)
        {
            switch (level)
            {
                case 0: return;
                case 1:
                    StartCoroutine(AttackCoroutine(Lvl1Attack));
                    break;
                case 2:
                    StartCoroutine(AttackCoroutine(Lvl2Attack));
                    break;
                case 3:
                    StartCoroutine(AttackCoroutine(Lvl3Attack));
                    break;
                case 4:
                    StartCoroutine(AttackCoroutine(Lvl4Attack));
                    break;
                /*case 5:
                   // StartCoroutine(AttackCoroutine(Lvl5Attack));
                    break;*/
            }
        }

        private IEnumerator AttackCoroutine(Action attack)
        {
            while (enabled)
            {
                yield return _delay;
                attack.Invoke();
                yield return new WaitForSeconds(Random.Range(_attackDelayMin, _attackDelayMax));
            }
        }


        private void Lvl1Attack()
        {
            var position = Random.insideUnitCircle;
            var projectile = Instantiate(_enemyProjectile, position + (Vector2) transform.position,
                Quaternion.identity);
            projectile.Initialize(position);
            projectile = Instantiate(_enemyProjectile, -position + (Vector2) transform.position, Quaternion.identity);
            projectile.Initialize(-position);
        }
        /*private void Lvl2Attack()
        {
            var playerPosition = FindObjectOfType<Player>().transform.position;
            var direction = playerPosition- transform.position ;
            var projectile = Instantiate(_enemyProjectile, (Vector2) transform.position,
                Quaternion.identity);
            projectile.Initialize(direction.normalized);
        }*/

        private void Lvl2Attack()
        {
            for (int i = 0; i < 3; i++)
            {
                var position = Random.insideUnitCircle;
                var projectile = Instantiate(_enemyProjectile, position + (Vector2) transform.position,
                    Quaternion.identity);
                projectile.Initialize(position);
            }
        }

        private void Lvl3Attack()
        {
            var attacks = new Vector2[]
            {
                new(1, 1),
                new(-1, +1),
                new(+1, -1),
                new(-1, -1),
            };
            foreach (var attack in attacks)
            {
                var projectile = Instantiate(_enemyProjectile, (Vector2) transform.position,
                    Quaternion.identity);
                projectile.Initialize(attack);
            }
        }

        private void Lvl4Attack()
        {
            var attacks = new Vector2[]
            {
                new(+1, +1),
                new(-1, +1),
                new(+1, -1),
                new(-1, -1),

                new(+1, 0),
                new(-1, 0),
                new(0, -1),
                new(0, +1),
            };
            foreach (var attack in attacks)
            {
                var projectile = Instantiate(_enemyProjectile, (Vector2) transform.position,
                    Quaternion.identity);
                projectile.Initialize(attack);
            }
        }

       

        public void NotifyDeath()
        {
            OnDie?.Invoke();
        }
    }
}