using System.Collections;
using UnityEngine;

namespace Components
{
    public class DestroyComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _objectToDestroy;
        [SerializeField] private bool _destroyThisGameObject;
        [SerializeField] private bool _destroyOnStartWithTime;
        [SerializeField] private float _destroyTime;

        private WaitForSeconds _delay;

        private void Awake()
        {
            _delay = new WaitForSeconds(_destroyTime);
        }

        private void Start()
        {
            if (_destroyOnStartWithTime)
            {
                StartCoroutine(DestroyInTime());
            }
        }

        public void Destroy()
        {
            Destroy(_destroyThisGameObject ? gameObject : _objectToDestroy);
        }

        private IEnumerator DestroyInTime()
        {
            yield return _delay;
            Destroy(_destroyThisGameObject ? gameObject : _objectToDestroy);
        }
    }
}