using System;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoader : MonoBehaviour
{
    public string SaveFile { get; set; }

    public void Load()
    {
        SaveManager.Load(SaveFile);
    }
}
