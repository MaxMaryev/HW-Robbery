using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robber : MonoBehaviour
{
    [SerializeField] private Transform _doorPoint;
    [SerializeField] private Transform _escapePoint;
    [SerializeField] private Transform _pointInHouse;
    [SerializeField] private GameObject _openedDoor;

    private float _time = 0;
    private int _speed = 5;
    private bool _isGoing = false;
    private bool _isRobbering = false;
    private bool _isRobbed = false;
    private Vector3 _initialScale;
    private Vector3 _smalledScale = new Vector3(0.5f, 0.5f, 0.5f);

    private void Awake()
    {
        _initialScale = transform.localScale;
    }

    private void Start()
    {
        StartCoroutine(LookAround());
    }

    private void Update()
    {
        if (_isGoing)
            GoRob();
        else if (_isRobbering)
            StartCoroutine(Rob());
        else if (_isRobbed)
            RunAway();
    }

    private IEnumerator LookAround()
    {
        int degreesTurnRight = 200;
        int degreesTurnLeft = 160;
        int timeForOneRotation = 1;
        int turnsNumber = 2;

        for (int i = 0; i < turnsNumber; i++)
        {

            while (_time <= timeForOneRotation)
            {
                DoOneRotation(_time, degreesTurnRight);
                yield return null;
            }

            while (_time > timeForOneRotation && _time <= timeForOneRotation + timeForOneRotation)
            {
                DoOneRotation(_time - timeForOneRotation, degreesTurnLeft);
                yield return null;
            }

            _time = 0;
            yield return null;
        }

        transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
        _isGoing = true;

        void DoOneRotation(float normilizedTime, int degrees)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(degrees, Vector3.up), normilizedTime);
            _time += Time.deltaTime;
        }
    }

    private IEnumerator Rob()
    {
        int timeForRoberry = 6;

        _isRobbering = false;
        yield return new WaitForSeconds(timeForRoberry);
        _isRobbed = true;
        ExitHouse();
    }

    private void GoRob()
    {
        if (transform.position != _doorPoint.transform.position)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, _smalledScale, Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, _doorPoint.transform.position, _speed * Time.deltaTime);
        }
        else
        {
            _isGoing = false;
            EnterHouse();
        }
    }

    private void ExitHouse()
    {
        int degreesOfViewDirection = 160;

        transform.position = _doorPoint.position;
        transform.rotation = Quaternion.AngleAxis(degreesOfViewDirection, Vector3.up);
    }

    private void EnterHouse()
    {
        _openedDoor.SetActive(true);
        transform.position = _pointInHouse.position;
        _isRobbering = true;
    }

    private void RunAway()
    {
        int speedRun = _speed + _speed;

        if (transform.position != _escapePoint.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, _escapePoint.position, speedRun * Time.deltaTime);
            transform.localScale = Vector3.Lerp(transform.localScale, _initialScale, Time.deltaTime);
        }
        else
        {
            _isRobbed = false;
        }
    }
}
