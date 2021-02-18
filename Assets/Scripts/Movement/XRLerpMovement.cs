using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class does the lerping movement
/// </summary>
public class XRLerpMovement : IXRMovement
{
    XRMovementSwitch control;

    int currentWPIndex = 0;
    bool moving = false;

    public void StartState(XRMovementSwitch control)
    {
        this.control = control;
        LerpMovementTrigger.onWaypointMove += MoveTowardsWP;
        Debug.Log("Chosen MovementType " + this);
    }
    public void ExitState()
    {
        Debug.Log("Exited MovementType " + this);
        LerpMovementTrigger.onWaypointMove -= MoveTowardsWP;
    }

    public void MoveTowardsWP(int newWpIndex)
    {     
        Debug.Log(newWpIndex + " <= " + control.Wps.Length);
        if(newWpIndex <= control.Wps.Length)
        {   

            control.movementVariables.lerpValue = 0f;
            currentWPIndex = newWpIndex;
            moving = true;
        }
    }
    
    public void UpdateState()
    {
        //TODO : Make movement based on waypoints and trigger movement somehow.
        if(control.Wps == null || control.Wps.Length == 0)
        {
            return;
        }

        if(moving)
        {
            control.movementVariables.lerpValue += Time.deltaTime * control.movementVariables.lerpSpeed;
            control.rig.transform.position = Vector3.Lerp(control.rig.transform.position, control.Wps[currentWPIndex].position, control.movementVariables.lerpValue);
            if(control.movementVariables.lerpValue >= 1.0f)
            {
                control.movementVariables.lerpValue = 1.0f;
                moving = false;
            }
        }
    }
}
