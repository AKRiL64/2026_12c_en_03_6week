using UnityEngine;
using UnityEngine.Serialization;

public class ScriptedActionTrigger: MonoBehaviour
{
    [FormerlySerializedAs("CutsceneObject")] public ScriptedActionObject scriptedActionObject;
    private bool wasTriggered = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Try Trigger" + this.name + " with " + other.name);
        if (wasTriggered || scriptedActionObject == null || !other.CompareTag("Player")) return;
        scriptedActionObject.TriggerCutsceneAction(this);
        wasTriggered = true;
        Debug.Log("Triggered" + other.name);
    }
}
