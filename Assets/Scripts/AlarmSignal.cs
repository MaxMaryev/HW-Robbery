using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmSignal : MonoBehaviour
{
    private float _runningTime;
    private bool _isWork = false;
    private float _maxVolume = 1;
    private float _minVolume = 0.3f;
    private float _durationOfVolumeChange = 1.2f;
    private AudioSource _sound;

    private void Awake()
    {
        _sound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        _isWork = true;
        _sound.Play();
    }

    private void OnTriggerExit(Collider other)
    {
        _isWork = false;
        _sound.Stop();
    }

    private void Update()
    {
        if (_isWork)
            RaiseAndDownVolume();
    }

    private void RaiseAndDownVolume()
    {
        int volumeChangesInFullCycle = 2;
        float cycleTime = volumeChangesInFullCycle * _durationOfVolumeChange;
        float normilizedTime = _runningTime / _durationOfVolumeChange;

        _runningTime += Time.deltaTime;

        if (_runningTime <= _durationOfVolumeChange)
        {
            _sound.volume = Mathf.MoveTowards(_maxVolume, _minVolume, normilizedTime);
        }
        else
        {
            _sound.volume = Mathf.MoveTowards(_minVolume, _maxVolume, normilizedTime - 1);
        }

        if (_runningTime > cycleTime)
            _runningTime = 0;
    }
}
