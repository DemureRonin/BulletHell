using Unity.Mathematics;
using UnityEngine;

namespace Enemy
{
    public class EnemySpawnWarning : MonoBehaviour
    {
        [SerializeField] private GameObject _enemyPrefab;
        private uint _level;
        public void Initialize(uint level)
        {
            _level = level;
        }

        public void SpawnEnemy()
        {
            var obj = Instantiate(_enemyPrefab, transform.position, quaternion.identity);
            obj.GetComponent<Enemy>().Initialize(_level);
            Destroy(gameObject);
        }
    }
}