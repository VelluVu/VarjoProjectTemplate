using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// This class does the teleportation movement.
/// </summary>
public class XRTeleportMovement : IXRMovement
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
        //TODO : Make teleport movement that works with headset button.

        RaycastHit hit;
        bool hits = Physics.Raycast(control.rig.hmd.position, control.rig.hmd.forward, out hit, control.movementVariables.rayCastDistance, control.movementVariables.teleMask);

        if(hits)
        {
            
        }
    }
}
