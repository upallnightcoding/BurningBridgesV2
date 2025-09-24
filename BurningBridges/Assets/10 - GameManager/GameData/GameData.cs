using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Bridge Burning/GameData")]
public class GameData : ScriptableObject
{
    // Distance from player to enemy
    public float enemyTargetDistance = 30.0f;

    // Island Percentage values
    //-------------------------
    public float healthPercent = 10.0f;
    public float enemiesPercent = 80.0f;

    // Number points when player is hit by skulls
    public int playerHitPoints = 2;

    // Distance betwween islands
    public float islandDistance = 30.69f;

    // Game startup default level
    public int defaultGameLevel = 5;

    public readonly string PLAYER_NAME_TAG = "Player";
}
