using System;

[Serializable]
public class TriggerEntry
{
    public ScriptedActionTrigger trigger;
    public int actionId;
}

[Serializable]
public class MoveActionEntry
{
    public int id;
    public ScriptedActionMove action;
}

[Serializable]
public class TeleportActionEntry
{
    public int id;
    public ScriptedActionTeleport action;
}

[Serializable]
public class DialogueActionEntry
{
    public int id;
    public ScriptedActionDialogue action;
}