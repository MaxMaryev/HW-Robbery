using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AlarmSignal : MonoBehaviour
{
    [SerializeField] private Robber _robber;

    private AudioSource _sound;
    private float _maxVolume = 1;
    private float _minVolume = 0.3f;
    private bool _isWorkStarted;
    private bool _isShutdownStarted;
    private Coroutine _upVolume;
    private Coroutine _downVolume;

    private void Awake()
    {
        _sound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == _robber && _isWorkStarted == false)
            StartCoroutine(Work());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == _robber && _isShutdownStarted == false)
            StartCoroutine(Shutdown());
    }

    private IEnumerator Work()
    {
        _isWorkStarted = true;

        _sound.Play();

        while (_isWorkStarted)
        {
            yield return _upVolume = StartCoroutine(ChangeVolume(_maxVolume));
            yield return _downVolume = StartCoroutine(ChangeVolume(_minVolume));
        }

        _isWorkStarted = false;
    }

    private IEnumerator ChangeVolume(float targetVolume)
    {
        while (_sound.volume != targetVolume)
        {
            _sound.volume = Mathf.MoveTowards(_sound.volume, targetVolume, Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator Shutdown()
    {
        _isShutdownStarted = true;

        if (_upVolume != null)
            StopCoroutine(_upVolume);
        if (_downVolume != null)
            StopCoroutine(_downVolume);

        yield return StartCoroutine(ChangeVolume(0));

        _sound.Stop();
        _isShutdownStarted = false;
    }
}
