using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Bridge Burning/GameData")]
public class GameData : ScriptableObject
{
    [Header("Game Distances ...")]
    public float enemyTargetDistance = 30.0f;
    public float islandDistance = 30.69f;

    [Header("Environment Token Percentage ...")]
    public float healthPercent = 10.0f;
    public float enemiesPercent = 80.0f;

    [Header("Player Hit Points ...")]
    public int playerHitPoints = 3;
    public int playerBridgeHitPoints = 1;

    // Game startup default level
    public int defaultGameLevel = 5;

    public float arrowDirectionTimer = 5.0f;

    public int nArrowHints = 3;

    public readonly string PLAYER_NAME_TAG = "Player";
}
