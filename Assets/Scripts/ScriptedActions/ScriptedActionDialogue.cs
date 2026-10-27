using System;
using UnityEngine;

[Serializable]
public class ScriptedActionDialogue: ScriptedAction
{
    public string DialogueText;
    public bool ShowDialogueBubble;
    
    public override void Execute(GameObject obj)
    {
        if (ShowDialogueBubble)
        {
            CutsceneObject.dialogueBubble.SetText(DialogueText);
            CutsceneObject.dialogueBubble.Show();
        }
        else
        {
            CutsceneObject.dialogueBubble.Hide();
        }
    }
}
