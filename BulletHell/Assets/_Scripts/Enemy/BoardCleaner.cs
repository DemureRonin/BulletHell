using System;
using PlayerScripts;
using UnityEngine;

namespace Enemy
{
    public class BoardCleaner : MonoBehaviour
    {
        private void OnEnable()
        {
            PlayerDeathObserver.OnDie += Clean;
        }

        private void Clean()
        {
            var enemies = FindObjectsOfType<Enemy>();
            foreach (var enemy in enemies)
            {
                Destroy(enemy.gameObject);
            }
            var enemiesSpawns = FindObjectsOfType<EnemySpawnWarning>();
            foreach (var enemy in enemiesSpawns)
            {
                Destroy(enemy.gameObject);
            }

            var food = FindObjectsOfType<FoodCollectable>();
            foreach (var obj in food)
            {
                Destroy(obj.gameObject);
            }
            var projectile = FindObjectsOfType<Projectile>();
            foreach (var obj in projectile)
            {
                Destroy(obj.gameObject);
            }
        }

        private void OnDisable()
        {
            PlayerDeathObserver.OnDie -= Clean;
        }
    }
}