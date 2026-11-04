using System.Collections.Generic;
using UnityEngine;

public class CutsceneDialogueDirector : MonoBehaviour
{
    [SerializeField] private TextAsset dialogueFile;
    [SerializeField] private DialogueWindow dialogueWindow;
    
    private static CutsceneDialogueDirector instance;

    private List<DialogueLine> dialogueLines = new List<DialogueLine>();
    private int currentLine = -1;

    private void Awake()
    {
        ParseDialogue();
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static CutsceneDialogueDirector GetInstance()
    {
        return instance;
    }

    private void ParseDialogue()
    {
        dialogueLines.Clear();

        string[] lines = dialogueFile.text.Split('\n');

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            string[] parts = line.TrimEnd('\r').Split('|', 2);

            if (parts.Length < 2)
            {
                Debug.LogWarning($"Invalid dialogue line: {line}");
                continue;
            }

            dialogueLines.Add(new DialogueLine(
                parts[0].Trim(),
                parts[1].Trim()
            ));
        }
    }

    public void ShowDialogue()
    {
        dialogueWindow.Show();
    }

    public void HideDialogue()
    {
        dialogueWindow.Hide();
    }

    public void NextLine()
    {
        if (dialogueLines.Count == 0)
        {
            Debug.LogWarning("No Dialogue Lines Available");
            return;
        }

        currentLine++;

        if (currentLine >= dialogueLines.Count)
        {
            Debug.LogWarning("No More Dialogue Lines");
            return;
        }

        dialogueWindow.SetDialogue(dialogueLines[currentLine]);
        ShowDialogue();
    }
}