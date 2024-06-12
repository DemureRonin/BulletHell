using UnityEngine;

namespace PlayerScripts
{
    public class PlayerDeathObserver : MonoBehaviour
    {
        public delegate void PlayerEvent();

        public static event PlayerEvent OnDie;
        public static event PlayerEvent OnStart;

        public void OnPlayerDeath()
        {
            OnDie?.Invoke();
        }
    }
}