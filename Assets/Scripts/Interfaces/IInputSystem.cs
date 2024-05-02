using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputSystem
{
    public class OnMovementEventArgs
    {
        public Vector2 inputVector;
    }

    public event EventHandler<OnMovementEventArgs> OnMovement;
}
