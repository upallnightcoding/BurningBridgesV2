using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UiCntrl uiCntrl;
    [SerializeField] private MazeCntrl mazeCntrl;
    [SerializeField] private EnvironmentCntrl envirCntrl;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void NewGameMenuOption()
    {
        uiCntrl.RenderGamePlayPanel();

        StartCoroutine(NewGameMenu());
    }

    private IEnumerator NewGameMenu()
    {
        mazeCntrl.StartNewGame(envirCntrl.transform);

        yield return new WaitForEndOfFrame();

        envirCntrl.Create();
    }

    public void SettingsMenuOption()
    {
        uiCntrl.RenderSettingsPanel();
    }

    public void ChronicleMenuOption()
    {
        uiCntrl.RenderDirectionPanel();
    }

    public void SettingsBackButton()
    {
        uiCntrl.RenderMainMenuPanel();
    }

    public void QuitGame()
    {
        Application.Quit(0);
    }
}
