using UnityEngine;

namespace Enemy
{
    public class EnemyRedirect : MonoBehaviour
    {
        [SerializeField] private int _modX;
        [SerializeField] private int _modY;
        public void Redirect(GameObject obj)
        {
            var enemy = obj.GetComponentInParent<global::Enemy.Enemy>();
            if (enemy == null) return;
            enemy.Redirect(_modX,_modY);
        }
    }
}