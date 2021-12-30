using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmSignal : MonoBehaviour
{
    [SerializeField] private float _durationOfVolumeChange;

    private AudioSource _sound;
    private float _maxVolume = 1;
    private float _minVolume = 0.3f;
    private bool _isChangingVolumeSwitchStatus = false;
    private bool _isWork = false;
    private bool _isSmoothShutdown = false;

    private void Awake()
    {
        _sound = GetComponent<AudioSource>();

        if (_durationOfVolumeChange <= 0)
        {
            _durationOfVolumeChange = 0.01f;
            Debug.Log("! јвтоматически выставлено минимальное значение продолжительности изменени€ громкости");
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
            RaiseAndDownVolume();

        if (_isSmoothShutdown)
        {
            ChangeVolume(0);

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
        ChangeVolume(_maxVolume);
        else
        ChangeVolume(_minVolume);
    }

    private void ChangeVolume(float finalVolume)
    {
        if (_sound.volume != finalVolume)
            _sound.volume = Mathf.MoveTowards(_sound.volume, finalVolume, Time.deltaTime);
        else
            _isChangingVolumeSwitchStatus = !_isChangingVolumeSwitchStatus;
    }
}
