using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static Action OnGunshotFired;

    public static void GunshotFired()
    {
        OnGunshotFired?.Invoke();
    }
}
