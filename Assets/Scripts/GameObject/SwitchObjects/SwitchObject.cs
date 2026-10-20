using System;
using UnityEngine;

public abstract class SwitchObject: MonoBehaviour
{
    protected enum SwitchState
    {
        On,
        Off
    }
    [SerializeField] private Lever assignedLever;
    [SerializeField] private SwitchState switchState;
    protected bool IsInitializing { get; private set; }
    
    private void Start()
    {
        if (assignedLever != null) assignedLever.OnLeverSwitch += Switch;
        
        IsInitializing =  true;
        if (switchState == SwitchState.On)
        {
            SwitchOn();
        }
        else
        {
            SwitchOff();
        }
        IsInitializing = false;
    }

    private void Switch()
    {
        if (switchState == SwitchState.On)
        {
            switchState = SwitchState.Off;
            SwitchOff();
        }
        else
        {
            switchState = SwitchState.On;
            SwitchOn();
        }
    }
    protected abstract void SwitchOn();
    protected abstract void SwitchOff();

    private void OnDisable()
    {
        if (assignedLever != null) assignedLever.OnLeverSwitch -= Switch;
    }
}
