using System;
using UnityEngine;

public class Lever: MonoBehaviour
{
    public Action OnLeverSwitch;

    
    public void ToggleLever()
    {
        OnLeverSwitch?.Invoke();
    }
}
