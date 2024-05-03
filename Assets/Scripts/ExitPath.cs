using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPath : MonoBehaviour
{
    public static event EventHandler OnEnter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.gameObject.GetComponent<Player>())
        {
            OnEnter?.Invoke(this, EventArgs.Empty);
        }
    }
}
