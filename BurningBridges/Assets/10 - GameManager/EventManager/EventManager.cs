using UnityEngine;
using System;

public class EventManager
{
    public event Action OnEnteringTurningBox = delegate { };
    public void InvokeOnEnterTurningBox() => OnEnteringTurningBox.Invoke();

    public event Action OnExitingTurningBox = delegate { };
    public void InvokeOnExitingTurningBox() => OnExitingTurningBox.Invoke();

    public static EventManager Instance
    {
        get
        {
            if (aInstance == null)
            {
                aInstance = new EventManager();
            }

            return (aInstance);
        }
    }

    public static EventManager aInstance = null;
}
