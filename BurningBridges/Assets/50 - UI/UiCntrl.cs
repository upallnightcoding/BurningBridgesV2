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
    [SerializeField] private GameObject youWinPanel;
    [SerializeField] private GameObject youLosePanel;

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

    private int playerHealth = 100;

    public void RenderYouLoseBanner(bool onOff) => youLosePanel.SetActive(onOff);
    public void RenderYouWinBanner(bool onOff) => youWinPanel.SetActive(onOff);

    void Start()
    {
        RenderMainMenuPanel();
    }

    public void RenderMainMenuPanel()
    {
        UnRenderPanels();

        mainMenuPanel.SetActive(true);

        gameTimeSwitch = false;
    }

    public void RenderSettingsPanel()
    {
        UnRenderPanels();

        settingsPanel.SetActive(true);

        gameTimeSwitch = false;
    }

    public void RenderGamePlayPanel()
    {
        UnRenderPanels();

        playerHealth = 100;

        gamePlayPanel.SetActive(true);

        StartCoroutine(UpdateTiming());
    }

    private void UnRenderPanels()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        gamePlayPanel.SetActive(false);
        youWinPanel.SetActive(false);
        youLosePanel.SetActive(false);
    }

    /**
     * RenderPlayerWins() - 
     */
    public void RenderPlayerWins()
    {
        StartCoroutine(RenderPlayerWinBanner());
    }

    private IEnumerator RenderPlayerWinBanner()
    {
        RenderYouWinBanner(true);

        yield return new WaitForSeconds(3.0f);

        RenderYouWinBanner(false);
        RenderMainMenuPanel();
    }

    public void UpdatePlayerHealth(int value)
    {
        playerHealth -= value;

        healthSlider.value = playerHealth / 100.0f; 

        if (playerHealth <= 0)
        {
            StartCoroutine(RenderPlayerLoseBanner());
        }
    }

    private IEnumerator RenderPlayerLoseBanner()
    {
        RenderYouLoseBanner(true);

        yield return new WaitForSeconds(3.0f);

        RenderYouLoseBanner(false);
        RenderMainMenuPanel();
    }

    public void OnSliderValueChanged(float value)
    {
        gameLevelText.text = "Game Level: " + ((int)value).ToString();

        EventManager.Instance.InvokeOnLevelChange((int)value);
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
        EventManager.Instance.OnPlayerHit -= UpdatePlayerHealth;
        EventManager.Instance.OnPlayerWin -= RenderPlayerWins;
    }

    private void OnEnable()
    {
        EventManager.Instance.OnUpdateEnemyCount += UpdateEnemyCount;
        EventManager.Instance.OnPlayerHit += UpdatePlayerHealth;
        EventManager.Instance.OnPlayerWin += RenderPlayerWins;
    }
}
