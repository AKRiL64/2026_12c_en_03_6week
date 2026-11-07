using System;
using UnityEngine;

[Serializable]
public class ScriptedActionTeleport: ScriptedAction
{
	public Vector2Int newPosition;
	
	public override void Execute(GameObject obj)
	{
		obj.transform.position = new Vector3(newPosition.x, newPosition.y, obj.transform.position.z);
	}
}