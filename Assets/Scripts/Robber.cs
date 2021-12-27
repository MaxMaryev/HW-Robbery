using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robber : MonoBehaviour
{
    [SerializeField] private GameObject _doorPoint;
    [SerializeField] private GameObject _escapePoint;
    [SerializeField] private GameObject _openedDoor;

    private float _time = 0;
    private int _speed = 5;
    private bool _isLookedAround = false;
    private bool _isRobbed = false;
    private Vector3 _initialScale;
    private Vector3 _smalledScale = new Vector3(0.5f, 0.5f, 0.5f);

    private void Start()
    {
        _initialScale = transform.localScale;

        StartCoroutine(LookAround());
        StartCoroutine(GoToHouse());
        StartCoroutine(RunAway());
    }

    private IEnumerator LookAround()
    {
        int degreesTurnRight = 200;
        int degreesTurnLeft = 160;
        int timeForOneRotation = 1;
        int turnsNumber = 2;

        for (int i = 0; i < turnsNumber; i++)
        {

            while (_time <= 1)
            {
                DoOneRotation(_time, degreesTurnRight);
                yield return null;
            }

            while (_time > 1 && _time <= 2)
            {
                DoOneRotation(_time - timeForOneRotation, degreesTurnLeft);
                yield return null;
            }

            _time = 0;
            yield return null;
        }

        transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
        _isLookedAround = true;

        void DoOneRotation(float normilizedTime, int degrees)
        {
            transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.AngleAxis(degrees, Vector3.up), normilizedTime);
            _time += Time.deltaTime;
        }
    }

    private IEnumerator GoToHouse()
    {
        bool isWaitingMoment = true;

        while (isWaitingMoment)
        {
            if (_isLookedAround)
            {
                transform.position = Vector3.MoveTowards(transform.position, _doorPoint.transform.position, _speed * Time.deltaTime);

                if (transform.position != _doorPoint.transform.position)
                {
                    transform.localScale = Vector3.Lerp(transform.localScale, _smalledScale, Time.deltaTime);
                }
                else if (_isRobbed == false)
                {
                    Vector3 farAwayPointForHidingCharacter = new Vector3(50, 0, 0);

                    _openedDoor.SetActive(true);
                    transform.position = farAwayPointForHidingCharacter;
                    _isRobbed = true;
                    break;
                }
            }

            yield return null;
        }
    }

    private IEnumerator RunAway()
    {
        bool isWaitingMoment = true;
        int degreesOfViewDirection = 160;
        int timeToRob = 6;

        while (isWaitingMoment)
        {
            while (_isRobbed)
            {
                yield return new WaitForSeconds(timeToRob);
                transform.position = _doorPoint.transform.position;
                transform.rotation = Quaternion.AngleAxis(degreesOfViewDirection, Vector3.up);
                int speedRun = _speed * 2;

                while (true)
                {
                    if (transform.position != _escapePoint.transform.position)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, _escapePoint.transform.position, speedRun * Time.deltaTime);
                        transform.localScale = Vector3.Lerp(transform.localScale, _initialScale, Time.deltaTime);
                    }
                    else
                    {
                        break;
                    }

                    yield return null;
                }

                break;
            }

            yield return null;
        }
    }
}
