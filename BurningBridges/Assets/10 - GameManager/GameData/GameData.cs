using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Bridge Burning/GameData")]
public class GameData : ScriptableObject
{
    // Distance from player to enemy
    public float enemyTargetDistance = 30.0f;

    // Island Percentage values
    //-------------------------
    public float healthPercent = 20.0f;
    public float enemiesPercent = 60.0f;

    // Number points when player is hit by skulls
    public int playerHitPoints = 2;
}
