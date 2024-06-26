using System;
using System.Collections;
using System.Collections.Generic;
using Components;
using UnityEngine;

public class BlinkEffect : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _player;
    [SerializeField] private SpriteRenderer _shadow;
    [SerializeField] private HealthComponent _health;
    [SerializeField] private float _blinkDelay;

    private WaitForSeconds _delay;

    private void Awake()
    {
        _delay = new WaitForSeconds(_blinkDelay);
    }

    public void Blink()
    {
        StartCoroutine(BlinkCoroutine());
    }

    private IEnumerator BlinkCoroutine()
    {
        _health.enabled = false;
        for (int i = 0; i < 3; i++)
        {
            _player.enabled = false;
            _shadow.enabled = false;
            yield return _delay;
            _player.enabled = true;
            _shadow.enabled = true;
            yield return _delay;
        }
        _health.enabled = true;
    }
}