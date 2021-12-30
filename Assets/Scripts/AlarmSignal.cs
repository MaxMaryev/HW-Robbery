using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmSignal : MonoBehaviour
{
    [SerializeField] private float _durationOfVolumeChange;

    private AudioSource _sound;
    private float _runningTime;
    private float _lastSavedVolume;
    private float _maxVolume = 1;
    private float _minVolume = 0.3f;
    private bool _isChangingVolumeSwitchStatus = false;
    private bool _isWork = false;
    private bool _isSmoothShutdown = false;

    private void Awake()
    {
        _sound = GetComponent<AudioSource>();
        _lastSavedVolume = _sound.volume;

        if (_durationOfVolumeChange <= 0)
        {
            _durationOfVolumeChange = 0.01f;
            Debug.Log("! Автоматически выставлено минимальное значение продолжительности изменения звука сигнализации");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _isWork = true;
        _sound.Play();
    }

    private void OnTriggerExit(Collider other)
    {
        _isWork = false;
        _isSmoothShutdown = true;
    }

    private void Update()
    {
        if (_isWork)
        {
            RaiseAndDownVolume();
            _lastSavedVolume = _sound.volume;
        }

        if (_isSmoothShutdown)
        {
            ChangeVolume(_lastSavedVolume, 0);

            if(_sound.volume == 0)
            {
                _isSmoothShutdown = false;
                _sound.Stop();
            }
        }
    }

    private void RaiseAndDownVolume()
    {
        if(_isChangingVolumeSwitchStatus)
        ChangeVolume(_minVolume, _maxVolume);
        else
        ChangeVolume(_maxVolume, _minVolume);
    }

    private void ChangeVolume(float startVolume, float finalVolume)
    {
        float normalizedTime = _runningTime / _durationOfVolumeChange;
        _runningTime += Time.deltaTime;

        if (_sound.volume != finalVolume)
        {
            _sound.volume = Mathf.MoveTowards(startVolume, finalVolume, normalizedTime);
        }
        else
        {
            _isChangingVolumeSwitchStatus = !_isChangingVolumeSwitchStatus;
            _runningTime = 0;
        }
    }
}
