using System.Collections.Generic;
using System.Linq;
using Components;
using PlayerScripts;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject _heartPrefab;
    [SerializeField] private GameObject _heartLayout;
    [SerializeField] private GameObject _tutorialContainer;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private GameObject _recordContainer;
    [SerializeField] private GameObject _recordTextContainer;
    [SerializeField] private TextMeshProUGUI _recordText;

    private int _score;
    private int _record;
    private int _currentHealth = 3;

    private readonly List<GameObject> _hearts = new();

    private void Start()
    {
        _record = PlayerPrefs.GetInt("Record");
        _recordText.text = _record.ToString();
    }

    private void OnScoreChanged()
    {
        _score++;
        _scoreText.text = _score.ToString();
    }

    private void OnHPChanged(int health)
    {
        if (health == _currentHealth) return;

        if (health > _currentHealth)
        {
            if (_currentHealth >= 5) return;
            _hearts.Add(Instantiate(_heartPrefab, _heartLayout.transform));
            _currentHealth++;
        }

        if (health < _currentHealth)
        {
             if (_hearts.Count == 0) return;
            Destroy(_hearts.Last());
            _hearts.Remove(_hearts.Last());
            _currentHealth--;
        }
    }

    private void OnEnable()
    {
        HealthComponent.OnHealthChanged += OnHPChanged;
        Enemy.Enemy.OnDie += OnScoreChanged;
        GameStart.OnStart += OnStart;
        PlayerDeathObserver.OnDie += OnDie;
    }

    private void OnDie()
    {
        _recordContainer.SetActive(true);
        _recordTextContainer.SetActive(true);
        if (_record < _score)
        {
            _record = _score;
            PlayerPrefs.SetInt("Record", _record);
        }

        _recordText.text = _record.ToString();

        for (int i = 0; i < _hearts.Count; i++)
        {
            Destroy(_hearts.Last());
            _hearts.Remove(_hearts.Last());
        }
        _tutorialContainer.SetActive(true);
    }

    private void OnDisable()
    {
        HealthComponent.OnHealthChanged -= OnHPChanged;
        Enemy.Enemy.OnDie -= OnScoreChanged;
        GameStart.OnStart -= OnStart;
        PlayerDeathObserver.OnDie -= OnDie;
    }

    private void OnStart()
    {
        _score = 0;
        _scoreText.text = _score.ToString();
        _recordContainer.SetActive(false);
        _recordTextContainer.SetActive(false);
        _currentHealth = 3;
        for (int i = 0; i < _currentHealth; i++)
        {
            _hearts.Add(Instantiate(_heartPrefab, _heartLayout.transform));
        }
        _tutorialContainer.SetActive(false);
    }
}