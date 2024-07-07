using System;
using System.Collections;
using PlayerScripts;
using Unity.Mathematics;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;


namespace Enemy
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private Vector4 _spawnBounds;
        [SerializeField] private GameObject _enemyPrefab;
        [SerializeField] private GameObject _foodPrefab;

        [SerializeField] private float _minFoodDelay;
        [SerializeField] private float _maxFoodDelay;
        [SerializeField] private float _minEnemyDelay;
        [SerializeField] private float _maxEnemyDelay;

        [SerializeField] private AudioSource _source;

        private uint _level = 0;
        private uint _score;

        private int _maxEnemies = 1;
        private int _enemiesPresent;

        private bool _foodPresent;
        private bool _running = false;
        private int _mod = 3;
        
        private void StartGame()
        {
            _running = true;
            StartCoroutine(FoodSpawnCoroutine());
            StartCoroutine(EnemySpawnCoroutine());
            Debug.Log("_enemiesPresent");
        }

        private void OnLose()
        {
            _maxEnemies = 1;
            _mod = 3;
            _level = 0;
            _score = 0;
            _foodPresent = false;
            _running = false;
            _enemiesPresent = 0;
            StopAllCoroutines();
        }

        private IEnumerator EnemySpawnCoroutine()
        {
            while (_running)
            {
                if (_enemiesPresent == _maxEnemies) yield return new WaitUntil(() => _enemiesPresent < _maxEnemies);
               //Debug.Log(_enemiesPresent);
               // Debug.Log(_maxEnemies);
                var delay = Random.Range(_minEnemyDelay, _maxEnemyDelay);
                yield return new WaitForSeconds(delay);
                var obj = SpawnAtRandomPosition(_enemyPrefab);
                obj.GetComponent<EnemySpawnWarning>().Initialize((uint) Random.Range(0, _level));
                _enemiesPresent++;
                _source.Play();
            }
        }

        private void OnEnemyDeath()
        {
            _enemiesPresent--;
            _score++;

            if (_score % _mod == 0)
            {
                Debug.Log("_level" + _level);
                _score = 0;

                _level++;
                _level = (uint) Mathf.Clamp(_level, 0, 5);

                _mod += 1;
                _maxEnemies++;
                _maxEnemies = Mathf.Clamp(_maxEnemies, 0, 5);
            }
        }

        private IEnumerator FoodSpawnCoroutine()
        {
            while (_running)
            {
                var delay = Random.Range(_minFoodDelay, _maxFoodDelay);
                yield return new WaitForSeconds(delay);
                if (_foodPresent) continue;
                SpawnAtRandomPosition(_foodPrefab);
                _foodPresent = true;
            }
        }

        private void OnCollect()
        {
            _foodPresent = false;
        }

        public GameObject SpawnAtRandomPosition(GameObject obj)
        {
            var randX = Random.Range(_spawnBounds.x, _spawnBounds.z);
            var randY = Random.Range(_spawnBounds.y, _spawnBounds.w);
            var spawnPos = new Vector2(randX, randY);
            return Instantiate(obj, spawnPos, quaternion.identity);
        }

        private void OnEnable()
        {
            FoodCollectable.OnCollect += OnCollect;
            Enemy.OnDie += OnEnemyDeath;
            PlayerDeathObserver.OnDie += OnLose;
            GameStart.OnStart += StartGame;
        }


        private void OnDisable()
        {
            FoodCollectable.OnCollect -= OnCollect;
            Enemy.OnDie -= OnEnemyDeath;
            PlayerDeathObserver.OnDie -= OnLose;
            GameStart.OnStart -= StartGame;
        }
    }
}