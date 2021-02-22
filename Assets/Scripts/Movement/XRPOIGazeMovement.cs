using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRPOIGazeMovement : IXRMovement
{
    XRMovementSwitch control;
    bool canMove = false;

    public delegate void GazePOIMoveDelegate(Transform target);
    public static event GazePOIMoveDelegate onAbleToMove;
    public static event GazePOIMoveDelegate onUnableToMove;

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
        POIGazeMovement();
    }

    public void POIGazeMovement()
    {
        RaycastHit hit; 
        bool hits = Physics.Raycast(control.rig.hmd.transform.position, control.rig.hmd.transform.forward, out hit, control.movementVariables.raycastDistance, control.movementVariables.POIMask);
        
        if(hits)
        {
            bool closeToTarget = Vector3.Distance(control.rig.hmd.position, hit.transform.position) < 0.5f;
            
            if(!closeToTarget)
            {
                if(!canMove)
                {
                    canMove = true;
                    onAbleToMove?.Invoke(hit.transform);
                }
                if(control.rig.input.onPrimaryButtonPress)
                {
                    Vector3 moveDir = (hit.transform.position - control.rig.hmd.position).normalized;
                    moveDir = new Vector3(moveDir.x, control.rig.transform.forward.y,moveDir.z);
                    control.rig.transform.position += moveDir * control.movementVariables.moveSpeed * Time.deltaTime;
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
}
