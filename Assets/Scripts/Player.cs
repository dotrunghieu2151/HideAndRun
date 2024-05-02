using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 7f;
    [SerializeField] private float _rotationSpeed = 10f;
    private Vector3 _velocity;
    private Vector3 _direction;
    private IInputSystem _inputSystem;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        _inputSystem = GameInput.Instance;
        _inputSystem.OnMovement += Move;
    }

    private void Move(object sender, IInputSystem.OnMovementEventArgs args)
    {
        _direction = new(args.inputVector.x, 0f, args.inputVector.y);
    }

    // Update is called once per frame
    void Update()
    {
        float moveDistance = _moveSpeed * Time.fixedDeltaTime;
        _velocity = _direction * moveDistance;
    }

    private void FixedUpdate()
    {
        transform.forward = Vector3.Slerp(transform.forward, _direction, _rotationSpeed * Time.deltaTime);
        _rigidbody.MovePosition(_rigidbody.position + _velocity * Time.fixedDeltaTime);
    }
}
