using UnityEngine;
using System;

public class EventManager
{
    public event Action OnPlayerPosition = delegate {};
    public void InvokeOnPlayerPosition() => OnPlayerPosition.Invoke();

    public event Action<int> OnUpdateEnemyCount = delegate { };
    public void InvokeOnUpdateEnemyCount(int count) => OnUpdateEnemyCount.Invoke(count);

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
