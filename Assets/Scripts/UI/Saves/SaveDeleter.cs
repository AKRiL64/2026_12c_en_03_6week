using System.IO;
using UnityEngine;

public class SaveDeleter : MonoBehaviour
{
    public string FilePath { get; set; }

    public void Delete()
    {
        File.Delete(FilePath);
        RetrieveSaves.Instance.Retrieve();
    }
}
