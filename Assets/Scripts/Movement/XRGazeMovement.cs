using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class does the Gaze movement.
/// </summary>
public class XRGazeMovement : IXRMovement
{
    XRMovementSwitch control;

    public delegate void GazePOIMoveDelegate(Transform target);
    public static event GazePOIMoveDelegate onAbleToMove;
    public static event GazePOIMoveDelegate onUnableToMove;

    public delegate void GazeMoveDelegate(Transform hmd);
    public static event GazeMoveDelegate onCorrectMoveAngle;
    public static event GazeMoveDelegate onNonCorrectMoveAngle;

    bool canMove = false;
    
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
        if(control.movementVariables.moveToPOI)
        {
            POIGazeMovement();
        }
        else
        {
            GazeMovement();
        }
    }

    private void GazeMovement()
    {
        float xAngle = control.rig.hmd.transform.rotation.eulerAngles.x;
       
        bool moving = xAngle < control.movementVariables.gazeAngleMax && xAngle > control.movementVariables.gazeAngleMin;
        RaycastHit hit;
        bool hits = Physics.Raycast(control.rig.hmd.position, control.rig.hmd.forward, out hit, 5f);

        if (moving)
        {  
            if(!canMove)
            {
                canMove = true;
                onCorrectMoveAngle?.Invoke(control.rig.hmd);
            }

            if(control.rig.onPrimaryButtonPress)
            {
                Vector3 moveDir = new Vector3(control.rig.hmd.transform.forward.x, control.rig.transform.forward.y, control.rig.hmd.transform.forward.z);
                control.rig.transform.position += moveDir * control.movementVariables.moveSpeed * Time.deltaTime;
                onCorrectMoveAngle?.Invoke(control.rig.hmd);    
            }
        }
        else
        {
            if(canMove)
            {
                canMove = false;
                onNonCorrectMoveAngle?.Invoke(control.rig.hmd);
            }
        }
    }

    public void POIGazeMovement()
    {
        RaycastHit hit; 
        bool hits = Physics.Raycast(control.rig.hmd.transform.position, control.rig.hmd.transform.forward, out hit, control.movementVariables.rayCastDistance, control.movementVariables.POIMask);
        
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
                if(control.rig.onPrimaryButtonPress)
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
