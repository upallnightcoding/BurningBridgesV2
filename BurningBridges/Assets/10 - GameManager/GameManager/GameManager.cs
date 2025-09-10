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

        StartCoroutine(CreateMaze());
    }

    private IEnumerator CreateMaze()
    {
        mazeCntrl.StartNewGame(envirCntrl.transform);

        yield return new WaitForEndOfFrame();

        envirCntrl.Create();
    }

    public void SettingsMenuOption()
    {
        uiCntrl.RenderSettingsPanel();
    }

    public void SettingsBackButton()
    {
        uiCntrl.RenderMainMenuPanel();
    }
}
