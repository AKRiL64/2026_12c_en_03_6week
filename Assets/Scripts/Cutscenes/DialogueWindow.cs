using TMPro;
using UnityEngine;

public class DialogueWindow: MonoBehaviour
{
    [SerializeField] private GameObject visuals;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI contentText;

    public void SetDialogue(DialogueLine line)
    {
        this.nameText.text = line.name;
        this.contentText.text = line.content;
    }
    
    public void Show()
    {
        visuals.SetActive(true);
    }

    public void Hide()
    {
        visuals.SetActive(false);
    }

}
