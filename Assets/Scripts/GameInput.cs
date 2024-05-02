using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour, IInputSystem
{
    public static GameInput Instance { get; private set; }
    public event EventHandler<IInputSystem.OnMovementEventArgs> OnMovement;


    private PlayerInput _playerInput;

    private void Awake()
    {
        Instance = this;
        _playerInput = new PlayerInput();
        _playerInput.Player.Enable();

        _playerInput.Player.Move.performed += Move_performed;
        _playerInput.Player.Move.canceled += Move_cancelled;
    }

    private void Move_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Vector2 inputVec = obj.ReadValue<Vector2>();
        OnMovement?.Invoke(this, new IInputSystem.OnMovementEventArgs { inputVector = inputVec.normalized });
    }

    private void Move_cancelled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnMovement?.Invoke(this, new IInputSystem.OnMovementEventArgs { inputVector = Vector2.zero });
    }
}
