using System.Collections.Generic;
using UnityEngine;

public static class CheckpointManager
{
    private static Dictionary<int, Vector3> _checkpoints = new Dictionary<int, Vector3>();
    private static GameObject _player;

    public static void SetPlayer(GameObject player)
    {
        _player = player;
    }

    public static void PutPlayerOnCheckpoint(int index)
    {
        if (_checkpoints.TryGetValue(index, out Vector3 position))
            _player.transform.position = position;
        else
            _player.transform.position = Vector3.zero;
    }
    
    public static void AddCheckpoint(int index, Vector3 position)
    {
        _checkpoints.TryAdd(index, position);
    }
    
    public static void WipeCheckpoints()
    {
        _checkpoints.Clear();
    }
}
