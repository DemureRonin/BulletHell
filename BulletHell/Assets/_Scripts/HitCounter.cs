using UnityEngine;

public class HitCounter : MonoBehaviour
{[SerializeField] private AudioSource _audio;
    [SerializeField] private AudioClip _2;
    [SerializeField] private AudioClip _3;
    private int _count;

    public delegate void CountEvent();

    public static event CountEvent OnValue2Reached;
    public static event CountEvent OnValue3Reached;

    public void Count()
    {
        _count++;
        switch (_count)
        {
            case 2:
                OnValue2Reached?.Invoke();
                _audio.PlayOneShot(_2);
                break;
            case 3:
                OnValue3Reached?.Invoke();
                _audio.PlayOneShot(_3);
                break;
        }
    }
}