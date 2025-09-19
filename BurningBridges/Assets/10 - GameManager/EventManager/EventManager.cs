using UnityEngine;
using System;

public class EventManager
{
    public event Action OnPlayerPosition = delegate {};
    public void InvokeOnPlayerPosition() => OnPlayerPosition.Invoke();

    public event Action<int> OnUpdateEnemyCount = delegate { };
    public void InvokeOnUpdateEnemyCount(int count) => OnUpdateEnemyCount.Invoke(count);

    public event Action<BridgeCntrl> OnResetPlayer = delegate { };
    public void InvokeOnResetPlayer(BridgeCntrl bridgeCntrl) => OnResetPlayer.Invoke(bridgeCntrl);

    public event Action<int> OnPlayerHit = delegate { };
    public void InvokeOnPlayerHit(int value) => OnPlayerHit.Invoke(value);

    public event Action OnPlayerWin = delegate { };
    public void InvokeOnPlayerWin() => OnPlayerWin.Invoke();

    // Event is raised when the user changes the game level
    //-----------------------------------------------------
    public event Action<int> OnLevelChange = delegate { };
    public void InvokeOnLevelChange(int value) => OnLevelChange.Invoke(value);

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
