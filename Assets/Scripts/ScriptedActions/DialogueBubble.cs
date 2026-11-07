using TMPro;
using UnityEngine;
public class DialogueBubble: MonoBehaviour
{
    [SerializeField] private GameObject visuals;
    [SerializeField] private TextMeshProUGUI textMesh;
    
    private string text;
    
    public void Show()
    {
        UpdateText();
        visuals.SetActive(true);
    }
    
    public void Hide()
    {
        visuals.SetActive(false);
    }

    public void SetText(string text)
    {
        this.text = text;
    }
    
    private void UpdateText()
    {
        textMesh.text = text;
    }
}
