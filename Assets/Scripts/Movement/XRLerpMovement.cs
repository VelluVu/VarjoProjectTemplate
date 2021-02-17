using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class does the lerping movement
/// </summary>
public class XRLerpMovement : IXRMovement
{
    XRMovementSwitch control;

    public void StartState(XRMovementSwitch control)
    {
        this.control = control;
        Debug.Log("Chosen MovementType " + this);
    }
    public void ExitState()
    {
        Debug.Log("Exited MovementType " + this);
    }
   
    public void UpdateState()
    {
        //TODO : Make movement based on waypoints and trigger movement somehow.
    }
}
