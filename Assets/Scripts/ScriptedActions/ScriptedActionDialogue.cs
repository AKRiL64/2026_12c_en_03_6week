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
            ScriptedActionObject.dialogueBubble.SetText(DialogueText);
            ScriptedActionObject.dialogueBubble.Show();
        }
        else
        {
            ScriptedActionObject.dialogueBubble.Hide();
        }
    }
}
