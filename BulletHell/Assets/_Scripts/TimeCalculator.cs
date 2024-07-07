using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCalculator : MonoBehaviour
{
    [SerializeField] private GameObject _barPivot;
    [SerializeField] private GameObject _volume;
    [SerializeField] private float _countdownTime;
    [SerializeField] private float _bulletTime;
    
    private const float MaxBarValue = 16.5f;

    private void Start()
    {
        StartCoroutine(CountdownToStop());
    }

    private IEnumerator CountdownToStop()
    {
        _volume.SetActive(false);
        var time = 0f;
        while (time < _countdownTime)
        {
            time += Time.deltaTime;
            var localScale = _barPivot.transform.localScale;
            localScale = new Vector3(localScale.x, MaxBarValue * time / _countdownTime, localScale.z);
            _barPivot.transform.localScale = localScale;
            yield return null;
        }

        StartCoroutine(StopTime());
    }

    private IEnumerator StopTime()
    {
        _volume.SetActive(true);
        var time = 0f;
        while (time < _bulletTime)
        {
            time += Time.deltaTime;
            var localScale = _barPivot.transform.localScale; 
            localScale = new Vector3(localScale.x, MaxBarValue * (_bulletTime-time) / _bulletTime, localScale.z);
            _barPivot.transform.localScale = localScale;
            yield return null;
        }
        StartCoroutine(CountdownToStop());
    }
}