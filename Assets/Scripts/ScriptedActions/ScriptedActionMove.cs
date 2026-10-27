using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class ScriptedActionMove: ScriptedAction
{
    public Vector2Int direction;
    public float time;
    public float distance;

    public override void Execute(GameObject obj)
    {
        CutsceneObject.StartCoroutine(MoveObject(obj));
    }

    IEnumerator MoveObject(GameObject obj)
    {
        Vector3 startPosition = obj.transform.position;

        Vector2 normalizedDirection = ((Vector2)direction).normalized;
        Vector3 targetPosition = startPosition + (Vector3)(normalizedDirection * distance);

        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            float progress = elapsedTime / time;

            obj.transform.position = Vector3.Lerp(
                startPosition,
                targetPosition,
                progress
            );

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        obj.transform.position = targetPosition;
    }
}
