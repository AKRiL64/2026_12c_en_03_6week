using System.Collections.Generic;
using UnityEngine;

public class CutsceneObject: MonoBehaviour
{
    public DialogueBubble dialogueBubble;
    
    [SerializeField] private List<TriggerEntry> triggerEntries = new List<TriggerEntry>();
    [SerializeField] private List<MoveActionEntry> moveActionEntries = new List<MoveActionEntry>();
    [SerializeField] private List<TeleportActionEntry> teleportActionEntries = new List<TeleportActionEntry>();
    [SerializeField] private List<DialogueActionEntry> dialogueActionEntries = new List<DialogueActionEntry>();

    private Dictionary<ScriptedActionTrigger, int> triggers;
    private Dictionary<int, ScriptedActionMove> moveActions;
    private Dictionary<int, ScriptedActionTeleport> teleportActions;
    private Dictionary<int, ScriptedActionDialogue> dialogueActions;

    private void Awake()
    {
        triggers = new Dictionary<ScriptedActionTrigger, int>();
        moveActions = new Dictionary<int, ScriptedActionMove>();
        teleportActions = new Dictionary<int, ScriptedActionTeleport>();
        dialogueActions = new Dictionary<int, ScriptedActionDialogue>();

        foreach (TriggerEntry entry in triggerEntries)
        {
            if (entry.trigger != null)
            {
                triggers[entry.trigger] = entry.actionId;
                entry.trigger.CutsceneObject = this;
            }
        }

        foreach (MoveActionEntry entry in moveActionEntries)
        {
            if (entry.action != null)
            {
                moveActions[entry.id] = entry.action;
                entry.action.CutsceneObject = this;
            }
        }

        foreach (TeleportActionEntry entry in teleportActionEntries)
        {
            if (entry.action != null) 
            {
                teleportActions[entry.id] = entry.action;
                entry.action.CutsceneObject = this;
            }
        }
        
        foreach (DialogueActionEntry entry in dialogueActionEntries)
        {
            if (entry.action != null) 
            {
                Debug.Log("Adding dialogue action " + entry.action.DialogueText);
                Debug.Log("Dialogue action ID: " + entry.id);
                dialogueActions[entry.id] = entry.action;
                entry.action.CutsceneObject = this;
            }
        }

    }
    
    public void TriggerCutsceneAction(ScriptedActionTrigger trigger)
    {
        Debug.Log("Triggered" + this.name);
        
        if (!triggers.TryGetValue(trigger, out int actionId))
            return;
        
        if (moveActions.TryGetValue(actionId, out ScriptedActionMove move))
            move.Execute(this.gameObject);
        
        if (teleportActions.TryGetValue(actionId, out ScriptedActionTeleport teleport))
            teleport.Execute(this.gameObject);
        
        if (dialogueActions.TryGetValue(actionId, out ScriptedActionDialogue dialogue))
            dialogue.Execute(this.gameObject);
    }
}