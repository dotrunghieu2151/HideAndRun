using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public event EventHandler OnPlayerHit;
    public event EventHandler OnPlayerNotHit;
    [SerializeField] private float _moveSpeed = 7f;
    [SerializeField] private float _rotationSpeed = 10f;
    private Vector3 _direction;
    private IInputSystem _inputSystem;
    private Rigidbody _rigidbody;

    private bool _isDisabled = false;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        _inputSystem = GameInput.Instance;
        _inputSystem.OnMovement += Move;

        GuardNavigation.OnPlayerSpotted += Disable;
        ExitPath.OnEnter += Disable;
    }

    private void OnDestroy()
    {
        GuardNavigation.OnPlayerSpotted -= Disable;
        ExitPath.OnEnter -= Disable;
    }

    private void Disable(object sender, EventArgs e)
    {
        _isDisabled = true;
    }

    private void Move(object sender, IInputSystem.OnMovementEventArgs args)
    {
        if (_isDisabled)
        {
            _direction = Vector3.zero;
            return;
        }

        _direction = new(args.inputVector.x, 0f, args.inputVector.y);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        _rigidbody.MovePosition(_rigidbody.position + _direction * _moveSpeed * Time.fixedDeltaTime);
        _rigidbody.MoveRotation(Quaternion.Slerp(_rigidbody.rotation, Quaternion.LookRotation(_direction), _rotationSpeed * Time.fixedDeltaTime * _direction.magnitude));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<GuardNavigation>(out GuardNavigation guard))
        {
            OnPlayerHit?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<GuardNavigation>(out GuardNavigation guard))
        {
            OnPlayerNotHit?.Invoke(this, EventArgs.Empty);
        }
    }
}
