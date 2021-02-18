using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class does the Gaze movement.
/// </summary>
public class XRGazeMovement : IXRMovement
{
    XRMovementSwitch control;

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
        GazeMovement();   
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

            if(control.rig.input.onPrimaryButtonPress)
            {
                Vector3 moveDir = new Vector3(control.rig.hmd.transform.forward.x, control.rig.transform.forward.y, control.rig.hmd.transform.forward.z);
                control.rig.transform.position += moveDir * control.movementVariables.moveSpeed * Time.deltaTime;
                //onCorrectMoveAngle?.Invoke(control.rig.hmd);    
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

    
}
