using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GuardNavigation : MonoBehaviour
{
    public static event EventHandler OnPlayerSpotted;
    [SerializeField] private Transform _paths;
    [SerializeField] private float _speed = 3;
    [SerializeField] private float _waitTime = 0.3f;
    [SerializeField] private float _turnSpeed = 3;
    [SerializeField] private Light _spotLight;

    [SerializeField] private float _viewDistance;
    [SerializeField] private Player _player;
    [SerializeField] private float _playerVisibleTimerMax = 0.5f;

    private float _playerVisibleTimer;
    private float _viewAngle;
    private Color _defaultSpotlightColor;

    private List<Vector3> _wayPoints;
    // Start is called before the first frame update

    private void Awake()
    {
        _viewAngle = _spotLight.spotAngle;
        _defaultSpotlightColor = _spotLight.color;
        _wayPoints = new List<Vector3>();

        foreach (Transform child in _paths)
        {
            _wayPoints.Add(new Vector3(child.position.x, gameObject.transform.position.y, child.position.z));
        }
    }


    void Start()
    {
        StartCoroutine(FollowPath(_wayPoints, _waitTime));
    }

    // Update is called once per frame
    void Update()
    {
        if (CanSeePlayer(_player))
        {
            _playerVisibleTimer += Time.deltaTime;
        }
        else
        {
            _playerVisibleTimer -= Time.deltaTime;
        }

        _playerVisibleTimer = Mathf.Clamp(_playerVisibleTimer, 0, _playerVisibleTimerMax);
        _spotLight.color = Color.Lerp(_defaultSpotlightColor, Color.red, _playerVisibleTimer);

        if (_playerVisibleTimer >= _playerVisibleTimerMax)
        {
            OnPlayerSpotted?.Invoke(this, EventArgs.Empty);
        }
    }

    private IEnumerator FollowPath(List<Vector3> wayPoints, float delayTime)
    {
        int wayPointIndex = 0;
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

    private bool CanSeePlayer(Player player)
    {
        return IsPlayerWithinViewingDistance(player) &&
            IsPlayerWithinViewingAngle(player) &&
            !IsThereAnyObstacleBetweenPlayerAndGuard(player);
    }

    private bool IsPlayerWithinViewingDistance(Player player)
    {
        return (player.transform.position - gameObject.transform.position).sqrMagnitude <= Mathf.Pow(_viewDistance, 2);
    }

    private bool IsPlayerWithinViewingAngle(Player player)
    {
        Vector3 dirVector = (player.transform.position - gameObject.transform.position).normalized;
        float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, dirVector);

        return angleBetweenGuardAndPlayer <= _viewAngle / 2f;
    }

    private bool IsThereAnyObstacleBetweenPlayerAndGuard(Player player)
    {
        Physics.Linecast(transform.position, player.transform.position, out RaycastHit hit);
        Player hitPlayer = hit.transform.gameObject.GetComponent<Player>();
        return hitPlayer == null || player != hitPlayer;
    }

    private IEnumerator TurnToFace(Vector3 lookTarget)
    {

        Vector3 directionVector = (lookTarget - transform.position).normalized;
        while (!((transform.forward - directionVector).sqrMagnitude <= 0.1f))
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
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * _viewDistance);
    }
}
