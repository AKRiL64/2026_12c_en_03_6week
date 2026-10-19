using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu: MonoBehaviour
{
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitButton;

    private void Start()
    {
        newGameButton.onClick.AddListener(NewGameButton);
        continueButton.onClick.AddListener(ContinueButton);
        settingsButton.onClick.AddListener(SettingsButton);
        exitButton.onClick.AddListener(ExitButton);
    }

    private void NewGameButton()
    {
        Debug.Log("New Game");
    }

    private void ContinueButton()
    {
        Debug.Log("Continue");
    }

    private void SettingsButton()
    {
        Debug.Log("Settings");
    }

    private void ExitButton()
    {
        Debug.Log("Exiting..");
        Application.Quit(0);
    }
    
}
