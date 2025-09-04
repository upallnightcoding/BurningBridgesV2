using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiCntrl : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private TMP_Text enemyCountText;

    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject gamePlayPanel;

    [SerializeField] private TMP_Text gameLevelText;

    private int enemyCount = 0;

    void Start()
    {
        RenderMainMenuPanel();
    }

    public void RenderMainMenuPanel()
    {
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
        gamePlayPanel.SetActive(false);
    }

    public void RenderSettingsPanel()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
        gamePlayPanel.SetActive(false);
    }

    public void RenderGamePlayPanel()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        gamePlayPanel.SetActive(true);
    }

    public void OnSliderValueChanged(float value)
    {
        gameLevelText.text = "Game Level: " + ((int)value).ToString();
    }

    private void UpdateEnemyCount(int count)
    {
        enemyCount += count;

        enemyCountText.text = enemyCount.ToString();
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
