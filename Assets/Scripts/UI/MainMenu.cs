using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu: MonoBehaviour
{
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button leaderboardButton;
    [SerializeField] private Button settingsBackButton;
    [SerializeField] private Button leaderboardBackButton;
    [SerializeField] private Button savesBackButton;
    [SerializeField] private Button savesContinueButton;
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject savesPanel;
    [SerializeField] private GameObject leaderboardPanel;
    [SerializeField] private GameObject confirmDialog;

    private void Start()
    {
        newGameButton.onClick.AddListener(NewGameButton);
        continueButton.onClick.AddListener(ContinueButton);
        settingsButton.onClick.AddListener(SettingsButton);
        settingsBackButton.onClick.AddListener(SettingsBackButton);
        leaderboardButton.onClick.AddListener(LeaderboardButton);
        leaderboardBackButton.onClick.AddListener(LeaderboardBackButton);
        savesBackButton.onClick.AddListener(SavesBackButton);
        savesContinueButton.onClick.AddListener(NewGameButton);
        exitButton.onClick.AddListener(OpenQuitDialog);
    }

    public void NewGameButton()
    {
        Debug.Log("New Game");
        SceneManager.LoadScene("Scenes/Levels/Tutorial");
    }

    private void ContinueButton()
    {
        Debug.Log("Continue");
        mainPanel.SetActive(false);
        savesPanel.SetActive(true);
    }

    private void SettingsButton()
    {
        Debug.Log("Settings");
        mainPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }
    
    private void SettingsBackButton()
    {
        Debug.Log("Main Menu");
        settingsPanel.SetActive(false);
        mainPanel.SetActive(true);
    }
    
    private void LeaderboardButton()
    {
        Debug.Log("Leaderboard");
        mainPanel.SetActive(false);
        leaderboardPanel.SetActive(true);
    }
    
    private void LeaderboardBackButton()
    {
        Debug.Log("Leaderboard Back");
        leaderboardPanel.SetActive(false);
        mainPanel.SetActive(true);
    }
    
    private void SavesBackButton()
    {
        Debug.Log("Saves Back");
        savesPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    public void OpenQuitDialog()
    {
        confirmDialog.SetActive(true);
    }

    public void CloseQuitDialog()
    {
        confirmDialog.SetActive(false);
    }

    public void ConfirmQuitDialog()
    {
        Debug.Log("Exiting...");
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit(0);
        #endif
    }
}
