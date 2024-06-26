using System;
using PlayerScripts;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    [SerializeField] private Collider2D _collider;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private SpriteRenderer _shadow;
    public delegate void PlayerEvent();

    public static event PlayerEvent OnStart;

    public void StartGame()
    {
        OnStart?.Invoke();
    }

    private void OnEnable()
    {
        PlayerDeathObserver.OnDie += OnDie;
    }

    private void OnDie()
    {
        _collider.enabled = true;
        _renderer.enabled = true;
        _shadow.enabled = true;
    }

    private void OnDisable()
    {
        PlayerDeathObserver.OnDie -= OnDie;
    }
}