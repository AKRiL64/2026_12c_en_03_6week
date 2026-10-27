using UnityEngine;

public abstract class ScriptedAction
{
    public CutsceneObject CutsceneObject;
    public abstract void Execute(GameObject obj);
}