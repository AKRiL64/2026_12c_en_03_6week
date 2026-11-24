
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneDialogueDirector : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private string nextScene;
    [SerializeField] private TextAsset dialogueFile;
    [SerializeField] private DialogueWindow dialogueWindow;
    [SerializeField] private PlayerInputHandler input;

    [Header("Dialogue Settings")]
    [SerializeField] private float secondsPerCharacter = 0.03f;

    private static CutsceneDialogueDirector instance;

    private List<DialogueLine> dialogueLines = new List<DialogueLine>();

    private int currentLine = -1;

    private Coroutine typingCoroutine;
    private bool isTyping;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        input.jumpPressed += NextLine;

        ParseDialogue();
        DisplayNextLine();
    }

    private void OnDestroy()
    {
        if (input != null)
            input.jumpPressed -= NextLine;

        if (instance == this)
            instance = null;
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
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] parts = line.TrimEnd('\r').Split('|', 2);

            if (parts.Length < 2)
            {
                Debug.LogWarning($"Invalid dialogue line: {line}");
                continue;
            }

            dialogueLines.Add(new DialogueLine(parts[0].Trim(), parts[1].Trim()));
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
        if (isTyping)
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
            }

            isTyping = false;

            dialogueWindow.SetVisibleCharacters(dialogueWindow.CharacterCount);

            return;
        }

        DisplayNextLine();
    }

    private void DisplayNextLine()
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
            NextScene();
            return;
        }

        dialogueWindow.SetDialogue(dialogueLines[currentLine]);
        ShowDialogue();

        dialogueWindow.SetVisibleCharacters(0);

        isTyping = true;
        typingCoroutine = StartCoroutine(TypeLine());
    }

    private IEnumerator TypeLine()
    {
        int characterCount = dialogueWindow.CharacterCount;

        for (int i = 1; i <= characterCount; i++)
        {
            dialogueWindow.SetVisibleCharacters(i);

            yield return new WaitForSecondsRealtime(secondsPerCharacter);
        }

        isTyping = false;
        typingCoroutine = null;
    }

    private void NextScene()
    {
        if (string.IsNullOrWhiteSpace(nextScene))
        {
            Debug.LogWarning("Next scene not specified");
            return;
        }

        SceneManager.LoadScene(nextScene);
    }
}
