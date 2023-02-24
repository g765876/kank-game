using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GlobalEventManager : MonoBehaviour
{
    public event EventHandler OnShoot;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnShoot?.Invoke(this, EventArgs.Empty);
        }
    }
}
