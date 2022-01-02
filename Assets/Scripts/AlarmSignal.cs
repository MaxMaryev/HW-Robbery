using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AlarmSignal : MonoBehaviour
{
    [SerializeField] private Collider _robber;

    private AudioSource _sound;
    private float _maxVolume = 1;
    private float _minVolume = 0.3f;
    private bool _isWork;
    private Coroutine _upVolume;
    private Coroutine _downVolume;

    private void Awake()
    {
        _sound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == _robber)
            StartCoroutine(Work());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == _robber)
            StartCoroutine(Shutdown());
    }

    private IEnumerator Work()
    {
        _isWork = true;

        _sound.Play();

        while (_isWork)
        {
            yield return _upVolume = StartCoroutine(ChangeVolume(_maxVolume));
            yield return _downVolume = StartCoroutine(ChangeVolume(_minVolume));
        }
    }

    private IEnumerator ChangeVolume(float finalVolume)
    {
        while (_sound.volume != finalVolume)
        {
            _sound.volume = Mathf.MoveTowards(_sound.volume, finalVolume, Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator Shutdown()
    {
        if (_upVolume != null)
            StopCoroutine(_upVolume);
        if (_downVolume != null)
            StopCoroutine(_downVolume);

        yield return StartCoroutine(ChangeVolume(0));

        _sound.Stop();
    }
}
