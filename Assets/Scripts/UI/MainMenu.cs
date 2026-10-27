using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu: MonoBehaviour
{
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button settingsBackButton;
    [SerializeField] private Button leaderboardButton;
    [SerializeField] private Button leaderboardBackButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject leaderboardPanel;

    private void Start()
    {
        newGameButton.onClick.AddListener(NewGameButton);
        continueButton.onClick.AddListener(ContinueButton);
        settingsButton.onClick.AddListener(SettingsButton);
        settingsBackButton.onClick.AddListener(SettingsBackButton);
        leaderboardButton.onClick.AddListener(LeaderboardButton);
        leaderboardBackButton.onClick.AddListener(LeaderboardBackButton);
        exitButton.onClick.AddListener(ExitButton);
        
        settingsPanel.SetActive(false);
        leaderboardPanel.SetActive(false);
    }

    public void NewGameButton()
    {
        Debug.Log("New Game");
        SceneManager.LoadScene("Scenes/Levels/Tutorial");
    }

    private void ContinueButton()
    {
        Debug.Log("Continue");
        SceneManager.LoadScene("Scenes/Levels/Tutorial");
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

    private void ExitButton()
    {
        Debug.Log("Exiting...");
        Application.Quit(0);
    }
    
}
