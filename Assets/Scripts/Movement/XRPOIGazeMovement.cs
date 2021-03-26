using UnityEngine;

/// <summary>
/// @Author: Veli-Matti Vuoti
/// This class handles the Point of interest Gaze Movement.
/// </summary>
public class XRPOIGazeMovement : IXRMovement
{
    XRMovementSwitch control;
    bool canMove = false;

    public delegate void GazePOIMoveDelegate(Transform target);
    public static event GazePOIMoveDelegate onAbleToMove;
    public static event GazePOIMoveDelegate onUnableToMove;

    Transform currentTarget = null;

    public void StartState(XRMovementSwitch control)
    {
        this.control = control;
        Debug.Log("Chosen MovementType " + this);
    }
    public void ExitState()
    {
        onUnableToMove?.Invoke(currentTarget);
        Debug.Log("Exited MovementType " + this);
    }
   
    public void UpdateState()
    {
        POIGazeMovement();
    }

    /// <summary>
    /// This function handles the Point of interest gaze movement,
    /// by shooting ray and check the hit on POI layer.
    /// When hits and calculates the distance of target.
    /// If hits and not too close invokes some events,
    /// and if using controllers check controller primary input to call function Move Towards Point.
    /// else if not using controllers call Move Towards Point on hmd primary button press
    /// </summary>
    public void POIGazeMovement()
    {
        RaycastHit hit; 
        bool hits = Physics.Raycast(control.rig.hmd.transform.position, 
                                    control.rig.hmd.transform.forward, 
                                    out hit, 
                                    control.movementVariables.raycastDistance, 
                                    control.movementVariables.POIMask);
        
        if(hits)
        {
            bool closeToTarget = Vector3.Distance(control.rig.hmd.position, hit.transform.position) < 0.5f;
            currentTarget = hit.transform;
            if(!closeToTarget)
            {
                if(!canMove)
                {
                    canMove = true;
                    onAbleToMove?.Invoke(hit.transform);
                    control.MoveLock(canMove);
                }
                if(control.usingControllers)
                {
                    if(control.rig.input.GetPrimaryButtonControlPress(control.preferredHand))
                    {
                        MoveTowardsPoint(hit.transform);
                    }
                }
                else
                {
                    if(control.rig.input.hmdPrimaryButtonPress)
                    {
                        MoveTowardsPoint(hit.transform);
                    }
                }
            }
            else
            {
                if(canMove)
                {
                    canMove = false;
                    onUnableToMove?.Invoke(hit.transform);
                    control.MoveLock(canMove);
                }
            }
        }
        else
        {
            if(canMove)
            {
                canMove = false;
                onUnableToMove?.Invoke(hit.transform);
            }
        }
    }

    /// <summary>
    /// This function moves the player towards the point of interest,
    /// By adding direction vector * speed * deltatime to rig position.
    /// </summary>
    /// <param name="target"></param>
    void MoveTowardsPoint(Transform target)
    {
        Vector3 moveDir = (target.position - control.rig.hmd.position).normalized;
        moveDir = new Vector3(moveDir.x, control.rig.transform.forward.y,moveDir.z);
        control.rig.transform.position += moveDir * control.movementVariables.moveSpeed * Time.deltaTime;
    }
}
