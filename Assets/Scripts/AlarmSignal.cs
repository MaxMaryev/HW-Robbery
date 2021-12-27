using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmSignal : MonoBehaviour
{
    private float _runningTime;
    private bool _isWentOff = false;
    private float _maxVolume = 1;
    private float _minVolume = 0.5f;
    private float _durationOfVolumeChange = 1.2f;
    private int _crossingCount = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _crossingCount++;

        if (_crossingCount == 1)
        {
            _isWentOff = true;
            GetComponent<AudioSource>().Play();
        }
        else
        {
            _isWentOff = false;
            GetComponent<AudioSource>().Stop();
        }
    }

    private void Start()
    {
        StartCoroutine(HowlDown());
    }

    private void Update()
    {
        if (_isWentOff)
        _runningTime += Time.deltaTime;
    }

    private IEnumerator HowlDown()
    {
        float loopTime = 2 * _durationOfVolumeChange;

        while (true)
        {
            while (_isWentOff)
            {
                float normilizedTime = _runningTime / _durationOfVolumeChange;

                if (_runningTime <= _durationOfVolumeChange)
                {
                    GetComponent<AudioSource>().volume = Mathf.MoveTowards(_maxVolume, _minVolume, normilizedTime);
                    yield return null;
                }
                else
                {
                    GetComponent<AudioSource>().volume = Mathf.MoveTowards(_minVolume, _maxVolume, normilizedTime - 1);
                    yield return null;
                }

                if (_runningTime > loopTime)
                    _runningTime = 0;
            }

            yield return null;
        }
    }
}
