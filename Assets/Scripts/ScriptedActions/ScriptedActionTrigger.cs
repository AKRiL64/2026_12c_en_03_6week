using UnityEngine;

public class ScriptedActionTrigger: MonoBehaviour
{
    public CutsceneObject CutsceneObject;
    private bool wasTriggered = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Try Trigger" + this.name + " with " + other.name);
        if (wasTriggered || CutsceneObject == null || !other.CompareTag("Player")) return;
        CutsceneObject.TriggerCutsceneAction(this);
        wasTriggered = true;
        Debug.Log("Triggered" + other.name);
    }
}
