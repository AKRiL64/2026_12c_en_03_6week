using UnityEngine;

public abstract class ScriptedAction
{
    public ScriptedActionObject ScriptedActionObject;
    public abstract void Execute(GameObject obj);
}