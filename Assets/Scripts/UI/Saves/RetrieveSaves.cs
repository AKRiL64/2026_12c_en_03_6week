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
    
    public void Retrieve()
    {
        if (!Directory.Exists("Saves"))
            return; 
        string[] files = Directory.GetFiles("Saves");
        
        for (int i = 0; i < files.Length; i++)
        {
            int slashIndex = files[i].LastIndexOfAny("/\\".ToCharArray());
            saveSlots[i].text = files[i].Substring(slashIndex + 1);
            SaveLoader loaderButton = playButtons[i].GetComponent<SaveLoader>();
            loaderButton.SaveFile = saveSlots[i].text;
            playButtons[i].onClick.AddListener(loaderButton.Load);
        }
    }
}
