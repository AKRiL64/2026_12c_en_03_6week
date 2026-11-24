using TMPro;
using UnityEngine;

public class DialogueWindow : MonoBehaviour
{
    [SerializeField] private GameObject visuals;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI contentText;

    public int CharacterCount
    {
        get
        {
            contentText.ForceMeshUpdate();
            return contentText.textInfo.characterCount;
        }
    }

    public void SetDialogue(DialogueLine line)
    {
        this.nameText.text = line.name;
        this.contentText.text = line.content;

        contentText.maxVisibleCharacters = int.MaxValue;
    }

    public void SetVisibleCharacters(int count)
    {
        contentText.maxVisibleCharacters = count;
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

