using System;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RetrieveSaves : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] saveSlots;
    [SerializeField] private Button[] playButtons;
    [SerializeField] private Button[] deleteButtons;
    public static RetrieveSaves Instance;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void Retrieve()
    {
        if (!Directory.Exists("Saves"))
            return; 
        ResetButtons();
        string[] files = Directory.GetFiles("Saves");
        
        for (int i = 0; i < files.Length; i++)
        {
            int slashIndex = files[i].LastIndexOfAny("/\\".ToCharArray());
            saveSlots[i].text = files[i].Substring(slashIndex + 1);
            SaveLoader loaderButton = playButtons[i].GetComponent<SaveLoader>();
            loaderButton.SaveFile = saveSlots[i].text;
            playButtons[i].onClick.AddListener(loaderButton.Load);

            if (deleteButtons[i].TryGetComponent<SaveDeleter>(out  SaveDeleter deleter))
            {
                deleter.FilePath = files[i];
                deleteButtons[i].onClick.AddListener(deleter.Delete);
            }
        }
    }

    private void ResetButtons()
    {
        foreach (Button button in playButtons)
            button.onClick.RemoveAllListeners();
        foreach (Button button in deleteButtons)
            button.onClick.RemoveAllListeners();
        foreach (TextMeshProUGUI slot in saveSlots)
            slot.text = "";
    }
}
