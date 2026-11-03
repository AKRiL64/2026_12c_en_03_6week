using System;
using UnityEngine;

[Serializable]
public class DialogueAction: CutsceneAction
{
    [SerializeField] bool isNextLine;
    
    public override void Execute()
    {
        CutsceneDialogueDirector director = CutsceneDialogueDirector.GetInstance();
        
        if (director == null)
            return;
        
        if (isNextLine)
        {
            director.NextLine();
        }
        else
        {
            director.HideDialogue();
        }
    }
}

[Serializable]
public class CutsceneDialogueActionEntry
{
    public int Index;
    public DialogueAction Action;
}