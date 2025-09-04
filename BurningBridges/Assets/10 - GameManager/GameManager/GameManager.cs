using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UiCntrl uiCntrl;
    [SerializeField] private MazeCntrl mazeCntrl;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void NewGameMenuOption()
    {
        uiCntrl.RenderGamePlayPanel();
        mazeCntrl.StartNewGame();
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
