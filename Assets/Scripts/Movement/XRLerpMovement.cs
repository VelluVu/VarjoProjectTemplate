using UnityEngine;

/// <summary>
/// @Author: Veli-Matti Vuoti
/// This class handles the lerping movement
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

    /// <summary>
    /// This function is called,
    /// when LerpMovementTrigger on Waypoint Move event is invoked.
    /// Sets new target waypoint index.
    /// </summary>
    /// <param name="newWpIndex">new waypoint index</param>
    public void MoveTowardsWP(int newWpIndex)
    {     
        Debug.Log(newWpIndex + " <= " + control.Wps.Length);
        if(newWpIndex <= control.Wps.Length)
        {   

            control.movementVariables.lerpValue = 0f;
            currentWPIndex = newWpIndex;
            moving = true;
            control.MoveLock(moving);
        }
    }
    
    /// <summary>
    /// This function Moves player towards target waypoint, 
    /// if there are any waypoints, 
    /// and moving boolean is true.
    /// When reached the waypoint as linear interpolation value is 1.0, 
    /// then sets moving false and removes the move locking.
    /// </summary>
    public void UpdateState()
    {
        if(control.Wps == null || control.Wps.Length == 0)
        {
            return;
        }

        if(moving)
        {
            control.movementVariables.lerpValue += Time.deltaTime * control.movementVariables.lerpSpeed;
            control.rig.transform.position = Vector3.Lerp(control.rig.transform.position, 
                                                          control.Wps[currentWPIndex].position, 
                                                          control.movementVariables.lerpValue);
            
            if(control.movementVariables.lerpValue >= 1.0f)
            {
                control.movementVariables.lerpValue = 1.0f;
                moving = false;
                control.MoveLock(moving);
            }
        }
    }
}
