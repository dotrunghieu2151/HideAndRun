using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardNavigation : MonoBehaviour
{
    [SerializeField] private Transform _paths;
    [SerializeField] private float _speed;
    [SerializeField] private float _waitTime = 0.3f;
    [SerializeField] private float _turnSpeed = 90;

    private List<Vector3> _wayPoints;
    // Start is called before the first frame update

    private void Awake()
    {
        _wayPoints = new List<Vector3>();

        foreach (Transform child in _paths)
        {
            _wayPoints.Add(child.position);
        }
    }


    void Start()
    {
        StartCoroutine(FollowPath(_wayPoints, _waitTime));
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator FollowPath(List<Vector3> wayPoints, float delayTime)
    {
        transform.position = wayPoints[0];
        int wayPointIndex = 1;
        Vector3 targetWayPoint = wayPoints[wayPointIndex];
        transform.LookAt(targetWayPoint);

        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetWayPoint, _speed * Time.deltaTime);
            if (transform.position == targetWayPoint)
            {
                wayPointIndex = (wayPointIndex + 1) % wayPoints.Count;
                targetWayPoint = wayPoints[wayPointIndex];

                yield return new WaitForSeconds(delayTime);
                yield return StartCoroutine(TurnToFace(targetWayPoint));
            }
            yield return null;
        }
    }

    private IEnumerator TurnToFace(Vector3 lookTarget)
    {

        Vector3 directionVector = (lookTarget - transform.position).normalized;
        while (!(Vector3.Distance(transform.forward, directionVector) <= 0.1f))
        {
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, directionVector, _turnSpeed * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
            yield return null;
        }
    }

    void OnDrawGizmos()
    {
        Vector3 startPos = _paths.GetChild(0).position;
        Vector3 previousPos = startPos;

        foreach (Transform child in _paths)
        {
            Gizmos.DrawSphere(child.position, 0.5f);
            Gizmos.DrawLine(previousPos, child.position);
            previousPos = child.position;
        }

        Gizmos.DrawLine(previousPos, startPos);
    }
}
