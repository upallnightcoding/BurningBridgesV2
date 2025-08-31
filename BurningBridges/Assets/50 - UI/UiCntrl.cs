using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiCntrl : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private TMP_Text enemyCountText;

    private int enemyCount = 0;

    void Start()
    {
        
    }

    private void UpdateEnemyCount(int count)
    {
        enemyCount += count;

        enemyCountText.text = enemyCount.ToString() + "/" + gameData.nEnemies;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnUpdateEnemyCount -= UpdateEnemyCount;
    }

    private void OnEnable()
    {
        EventManager.Instance.OnUpdateEnemyCount += UpdateEnemyCount;
    }
}
