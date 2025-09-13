using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UiCntrl : MonoBehaviour
{
    [Header("Game Data ...")]
    [SerializeField] private GameData gameData;

    [Header("Panels ...")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject gamePlayPanel;

    [SerializeField] private TMP_Text gameLevelText;
    [SerializeField] private TMP_Text enemyCountText;

    [SerializeField] private Slider healthSlider;

    [SerializeField] private MazeCntrl mazeCntrl;
    [SerializeField] private TMP_Text gameTimeText;

    [Header("Mini Map ...")]
    [SerializeField] private RectTransform miniMapContainer;
    [SerializeField] private GameObject enemyPrefab;

    private float gameTimeCalc = 1.0f;
    private int gameTimeSec = 0;
    private bool gameTimeSwitch = true;

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

        gameTimeSwitch = false;
    }

    public void RenderSettingsPanel()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
        gamePlayPanel.SetActive(false);

        gameTimeSwitch = false;
    }

    public void RenderGamePlayPanel()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        gamePlayPanel.SetActive(true);

        UpdateMiniMap();

        StartCoroutine(UpdateTiming());
    }

    public void UpdateMiniMap()
    {
        float size = 5.0f;

        GameObject go = Instantiate(enemyPrefab);
        go.transform.SetParent(miniMapContainer, false);

        for (int w = 0; w < mazeCntrl.GetWidth(); w++)
        {
            for (int h = 0; h < mazeCntrl.GetHeight(); h++)
            {
            }
        }
    }

    public void SetHealth(float value)
    {
        float slide = value / 100.0f;

        healthSlider.value = slide;
    }

    public void OnSliderValueChanged(float value)
    {
        gameLevelText.text = "Game Level: " + ((int)value).ToString();
    }

    /**
     * UpdateTiming() - Update the time element on the UI by one second.  When
     * the game time switch is set to false the timing element will stop.  
     */
    private IEnumerator UpdateTiming()
    {
        gameTimeCalc = 1.0f;
        gameTimeSec = 0;
        gameTimeSwitch = true;

        while (gameTimeSwitch)
        {
            gameTimeCalc -= Time.deltaTime;

            if (gameTimeCalc < 0.0f)
            {
                gameTimeSec += 1;
                gameTimeText.text = gameTimeSec.ToString() + " sec";

                gameTimeCalc = 1.0f;
            }

            yield return null;
        }
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
