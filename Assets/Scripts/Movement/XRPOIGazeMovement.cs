using UnityEngine;

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
        //TODO : Make movement based on waypoints and trigger movement somehow.
        POIGazeMovement();
    }

    public void POIGazeMovement()
    {
        RaycastHit hit; 
        bool hits = Physics.Raycast(control.rig.hmd.transform.position, control.rig.hmd.transform.forward, out hit, control.movementVariables.raycastDistance, control.movementVariables.POIMask);
        
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

    void MoveTowardsPoint(Transform target)
    {
        Vector3 moveDir = (target.position - control.rig.hmd.position).normalized;
        moveDir = new Vector3(moveDir.x, control.rig.transform.forward.y,moveDir.z);
        control.rig.transform.position += moveDir * control.movementVariables.moveSpeed * Time.deltaTime;
    }
}
