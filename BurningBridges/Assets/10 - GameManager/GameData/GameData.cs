using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Bridge Burning/GameData")]
public class GameData : ScriptableObject
{
    public float enemyTargetDistance = 30.0f;

    public int nHealth = 3;

    public int nTreasure = 3;

    public int nEnemies = 7;
}
